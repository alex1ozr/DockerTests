using System.Net;
using System.Net.Http.Json;
using DockerTestsSample.Api.Contracts.Responses;
using DockerTestsSample.Api.IntegrationTests.Abstract;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.PersonController;

[Collection("Test collection")]
public class CreatePersonControllerTests : ControllerTestsBase
{
    public CreatePersonControllerTests(PersonApiFactory apiFactory)
        : base(apiFactory)
    {
    }

    [Fact]
    public async Task Create_CreatesUser_WhenDataIsValid()
    {
        // Arrange
        var personRequest = PersonGenerator.Generate();
        var personId = Guid.NewGuid();

        // Act
        var response = await _client.PostAsJsonAsync($"people/{personId}", personRequest);

        // Assert
        var personResponse = await response.Content.ReadFromJsonAsync<PersonResponse>();
        personResponse.Should().BeEquivalentTo(personRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should()
            .Be($"http://localhost/people/{personResponse!.Id}");
    }

    [Fact]
    public async Task Create_ReturnsValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        const string invalidEmail = "someInvalidEmail";
        var personRequest = PersonGenerator.Clone()
            .RuleFor(x => x.Email, invalidEmail).Generate();

        // Act
        var response = await _client.PostAsJsonAsync($"people/{Guid.NewGuid()}", personRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error.Errors.Should().ContainKey("Person.Email");
    }
}
