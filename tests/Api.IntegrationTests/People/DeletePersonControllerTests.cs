using DockerTestsSample.Api.IntegrationTests.Infrastructure;
using DockerTestsSample.Client.Implementations;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.People;

public sealed class DeletePersonControllerTests : ControllerTestsBase
{
    public DeletePersonControllerTests(TestApplication apiFactory)
        : base(apiFactory)
    {
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenPersonExists()
    {
        // Arrange
        var personRequest = PersonGenerator.Generate();
        var personId = Guid.NewGuid();

        await Client.People.CreatePersonAsync(personId, personRequest);

        // Act
        await Client.People.DeletePersonAsync(personId);

        // Assert
        var exception = await Client.People
           .Invoking(p => p.GetPersonAsync(personId))
           .Should().ThrowAsync<ApiException<ProblemDetails>>();

        exception.Which
            .Result
            .Status.Should()
            .Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // Act, Assert
        var exception = await Client.People
            .Invoking(p => p.DeletePersonAsync(Guid.NewGuid()))
            .Should().ThrowAsync<ApiException<ProblemDetails>>();

        exception.Which
            .Result
            .Status.Should()
            .Be(StatusCodes.Status404NotFound);
    }
}
