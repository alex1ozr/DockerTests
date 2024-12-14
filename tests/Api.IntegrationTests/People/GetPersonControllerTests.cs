using DockerTestsSample.Api.IntegrationTests.Infrastructure;
using DockerTestsSample.Client.Implementations;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.People;

public sealed class GetPersonControllerTests : ControllerTestsBase
{
    public GetPersonControllerTests(TestApplication apiFactory)
        : base(apiFactory)
    {
    }

    [Fact]
    public async Task Get_ReturnsPerson_WhenPersonExists()
    {
        // Arrange
        var personRequest = PersonGenerator.Generate();
        var personId = Guid.NewGuid();

        await Client.People.CreatePersonAsync(personId, personRequest);

        // Act
        var retrievedPerson = await Client.People.GetPersonAsync(personId);

        // Assert
        retrievedPerson.Should().BeEquivalentTo(personRequest);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // Act, Assert
        var exception = await Client.People
            .Invoking(p => p.GetPersonAsync(Guid.NewGuid()))
            .Should().ThrowAsync<ApiException<ProblemDetails>>();

        exception.Which
            .Result
            .Status.Should()
            .Be(StatusCodes.Status404NotFound);
    }
}
