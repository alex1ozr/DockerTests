using AutoMapper;
using DockerTestsSample.UnitTests.Di;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DockerTestsSample.UnitTests.Automapper;

/// <summary>
/// Tests for Automapper configuration
/// </summary>
public sealed class AutoMapperTests: IClassFixture<ContainerFixture>
{
    private readonly IServiceProvider _container;

    public AutoMapperTests(ContainerFixture fixture)
    {
        _container = fixture.Container;
    }
    
    [Fact(DisplayName = "Assert configuration valid")]
    public void AssertConfiguration_ShouldBeValid()
    {
        var mapper = _container.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}