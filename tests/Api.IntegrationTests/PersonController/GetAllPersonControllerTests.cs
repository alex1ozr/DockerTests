using System.Net;
using System.Net.Http.Json;
using DockerTestsSample.Api.Contracts.Responses;
using DockerTestsSample.Api.IntegrationTests.Abstract;
using FluentAssertions;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.PersonController;

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
        var person = PersonGenerator.Generate();
        var personId = Guid.NewGuid();

        var createdResponse = await HttpClient.PostAsJsonAsync($"people/{personId}", person);
        var createdPerson = await createdResponse.Content.ReadFromJsonAsync<PersonResponse>();

        // Act
        var response = await HttpClient.GetAsync("people");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var peopleResponse = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<PersonResponse>>();
        peopleResponse!.Single().Should().BeEquivalentTo(createdPerson);
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyResult_WhenNoPeopleExist()
    {
        // Act
        var response = await HttpClient.GetAsync("people");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var peopleResponse = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<PersonResponse>>();
        peopleResponse!.Should().BeEmpty();
    }
}
