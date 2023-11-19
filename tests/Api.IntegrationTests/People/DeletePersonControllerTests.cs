using DockerTestsSample.Api.IntegrationTests.Abstract;
using DockerTestsSample.Client.Implementations;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.People;

public sealed class DeletePersonControllerTests: ControllerTestsBase
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
        Func<Task> f = async () => await Client.People.GetPersonAsync(personId);
        var exception = await f.Should().ThrowAsync<ApiException<ProblemDetails>>();
        exception.Which
            .Result
            .Status.Should()
            .Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        Func<Task> f = async () => await Client.People.DeletePersonAsync(Guid.NewGuid());
        var exception = await f.Should().ThrowAsync<ApiException<ProblemDetails>>();
        exception.Which
            .Result
            .Status.Should()
            .Be(StatusCodes.Status404NotFound);
    }
}
