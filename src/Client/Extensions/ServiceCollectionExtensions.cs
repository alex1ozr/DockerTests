using DockerTestsSample.Client.Abstract;
using DockerTestsSample.Client.Implementations;
using DockerTestsSample.Client.Options;
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

    public static void AddSampleClient(
        this IServiceCollection services,
        Action<ClientOptions>? configureOptions = null)
    {
        services.AddClientOptions(configureOptions);

        services.AddHttpClient<IPersonClient, PersonClient>(
                (provider, client) =>
                {
                    var baseUrl = provider.GetRequiredService<IOptions<ClientOptions>>().Value.RequiredServerUrl;
                    client.BaseAddress = baseUrl;
                })
            .AddPolicyHandler(RetryPolicy);

        services.AddTransient<ISampleClient, SampleClient>();
    }

    private static IServiceCollection AddClientOptions(
        this IServiceCollection services,
        Action<ClientOptions>? configureOptions = null)
    {
        services.AddOptions<ClientOptions>()
            .BindConfiguration(ClientOptions.OptionKey)
            .ValidateDataAnnotations()
            .Configure(configureOptions ?? (_ => { }));

        return services;
    }
}
