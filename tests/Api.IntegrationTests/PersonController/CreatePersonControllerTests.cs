using System.Net;
using System.Net.Http.Json;
using DockerTestsSample.Api.IntegrationTests.Abstract;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.PersonController;

public sealed class CreatePersonControllerTests : ControllerTestsBase
{
    public CreatePersonControllerTests(TestApplication apiFactory)
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
        var result = await Client.People.CreatePersonAsync(personId, personRequest);

        // Assert
        result.Should().BeEquivalentTo(personRequest);
    }

    [Fact]
    public async Task Create_ReturnsValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        const string invalidEmail = "someInvalidEmail";
        var personRequest = PersonGenerator.Clone()
            .RuleFor(x => x.Email, invalidEmail).Generate();

        // Act
        var response = await HttpClient.PostAsJsonAsync($"people/{Guid.NewGuid()}", personRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error.Errors.Should().ContainKey("Person.Email");
    }
    
    [Fact]
    public async Task Create_ReturnsError_WhenPersonAlreadyExists()
    {
        // Arrange
        var personRequest = PersonGenerator.Generate();
        var personId = Guid.NewGuid();

        var createdResponse = await HttpClient.PostAsJsonAsync($"people/{personId}", personRequest);

        // Act
        var response = await HttpClient.PostAsJsonAsync($"people/{personId}", personRequest);

        // Assert
        createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        error!.Status.Should().Be((int) HttpStatusCode.Conflict);
        error.Type.Should().Be("person_already_exists");
    }
}
