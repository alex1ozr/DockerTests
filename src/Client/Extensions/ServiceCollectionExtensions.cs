using DockerTestsSample.Client.Implementations;
using DockerTestsSample.Client.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace DockerTestsSample.Client.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly IAsyncPolicy<HttpResponseMessage> RetryPolicy
        = HttpPolicyExtensions
            // Handle HttpRequestExceptions, 408 and 5xx status codes
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));
    

    public static void AddSampleClient(this IServiceCollection services)
    {
        services.AddClientOptions();

        services.AddHttpClient<IPersonClient, PersonClient>(
                (provider, client) =>
                {
                    var baseUrl = provider.GetRequiredService<IOptions<ClientOptions>>().Value.Url();
                    client.BaseAddress = baseUrl;
                })
            .AddPolicyHandler(RetryPolicy);

        services.AddTransient<IPersonClient, PersonClient>();
    }

    private static IServiceCollection AddClientOptions(this IServiceCollection services)
    {
        services.AddOptions<ClientOptions>()
            .BindConfiguration(ClientOptions.OptionKey)
            .ValidateDataAnnotations();

        return services;
    }
}
