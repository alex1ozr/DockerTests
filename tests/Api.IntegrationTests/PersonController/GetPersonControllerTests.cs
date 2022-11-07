using System.Net;
using System.Net.Http.Json;
using DockerTestsSample.Api.Contracts.Responses;
using DockerTestsSample.Api.IntegrationTests.Abstract;
using FluentAssertions;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.PersonController;

[Collection("Test collection")]
public class GetPersonControllerTests : ControllerTestsBase
{
    public GetPersonControllerTests(PersonApiFactory apiFactory)
        : base(apiFactory)
    {
    }

    [Fact]
    public async Task Get_ReturnsPerson_WhenPersonExists()
    {
        // Arrange
        var person = PersonGenerator.Generate();
        var personId = Guid.NewGuid();

        var createdResponse = await _client.PostAsJsonAsync($"people/{personId}", person);
        var createdPerson = await createdResponse.Content.ReadFromJsonAsync<PersonResponse>();

        // Act
        var response = await _client.GetAsync($"people/{createdPerson!.Id}");

        // Assert
        var retrievedPerson = await response.Content.ReadFromJsonAsync<PersonResponse>();
        retrievedPerson.Should().BeEquivalentTo(createdPerson);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync($"people/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
