using DockerTestsSample.Client.Abstract;
using DockerTestsSample.Client.Implementations;
using DockerTestsSample.Client.Options;
using Microsoft.Extensions.DependencyInjection;

namespace DockerTestsSample.Api.IntegrationTests.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSampleClientTest(this IServiceCollection services, Func<HttpClient> httpClientFactory)
    {
        services.AddClientOptions();

        services.AddTransient<IPersonClient>(_ => new PersonClient(httpClientFactory()));
        services.AddTransient<ISampleClient, SampleClient>();
    }

    private static IServiceCollection AddClientOptions(this IServiceCollection services)
    {
        services.AddOptions<ClientOptions>()
            .BindConfiguration(ClientOptions.OptionKey)
            .ValidateDataAnnotations();

        return services;
    }
}
