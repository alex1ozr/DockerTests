using DockerTestsSample.Api.IntegrationTests.Abstract;
using FluentAssertions;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.People;

public sealed class GetAllPersonControllerTests : ControllerTestsBase
{
    public GetAllPersonControllerTests(TestApplication apiFactory)
        : base(apiFactory)
    {
    }

    [Fact]
    public async Task GetAll_ReturnsAllPeople_WhenPeopleExist()
    {
        // Arrange
        var personRequest = PersonGenerator.Generate();
        var personId = Guid.NewGuid();

        await Client.People.CreatePersonAsync(personId, personRequest);

        // Act
        var response = await Client.People.GetAllPeopleAsync();

        // Assert
        response.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new[] { personRequest });
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyResult_WhenNoPeopleExist()
    {
        // Act
        var response = await Client.People.GetAllPeopleAsync();

        // Assert
        response.Should()
            .NotBeNull()
            .And.BeEmpty();
    }
}
