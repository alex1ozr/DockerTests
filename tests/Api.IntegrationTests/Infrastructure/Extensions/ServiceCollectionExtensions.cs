using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

namespace DockerTestsSample.Api.IntegrationTests.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Enable HttpClients to redirect requests to the TestServer
    /// </summary>
    public static IServiceCollection AddTestHosting(
        this IServiceCollection services,
        TestServer server)
    {
        services.TryAddSingleton(server);
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpMessageHandlerBuilderFilter, TestServerMessageFilter>());

        return services;
    }
}
