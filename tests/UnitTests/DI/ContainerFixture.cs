using DockerTestsSample.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DockerTestsSample.UnitTests.DI;

/// <summary>
/// Fixture for DI-container tests
/// </summary>
public sealed class ContainerFixture : WebApplicationFactory<IApiMarker>
{
    public IServiceProvider Container => Services;
}