using DockerTestsSample.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DockerTestsSample.UnitTests.DI;

/// <summary>
/// API dependencies tests
/// </summary>
public sealed class DependenciesTests : IClassFixture<ContainerFixture>
{
    private readonly IServiceProvider _container;

    public DependenciesTests(ContainerFixture fixture)
    {
        _container = fixture.Container;
    }

    [Theory(DisplayName = "Resolve controller")]
    [MemberData(nameof(Controllers))]
    internal void ControllerShouldBeResolved(Type controllerType)
    {
        var instance = _container.GetRequiredService(controllerType);

        instance.Should().NotBeNull();
    }

    /// <summary>
    /// All API controllers
    /// </summary>
    public static IEnumerable<object[]> Controllers =>
        typeof(PersonController).Assembly
            .DefinedTypes
            .Where(type => type.IsAssignableTo(typeof(ControllerBase)))
            .Where(type => !type.IsAbstract)
            .Select(type => new[] { type });
}