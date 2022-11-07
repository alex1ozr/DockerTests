using System.Net;
using System.Net.Http.Json;
using DockerTestsSample.Api.Contracts.Responses;
using DockerTestsSample.Api.IntegrationTests.Abstract;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.PersonController;

public class UpdatePersonControllerTests : ControllerTestsBase
{
    public UpdatePersonControllerTests(PersonApiFactory apiFactory) :
        base(apiFactory)
    {
    }

    [Fact]
    public async Task Update_UpdatesUser_WhenDataIsValid()
    {
        // Arrange
        var person = PersonGenerator.Generate();
        var personId = Guid.NewGuid();

        var createdResponse = await Client.PostAsJsonAsync($"people/{personId}", person);
        var createdPerson = await createdResponse.Content.ReadFromJsonAsync<PersonResponse>();

        person = PersonGenerator.Generate();

        // Act
        var response = await Client.PutAsJsonAsync($"people/{createdPerson!.Id}", person);

        // Assert
        var personResponse = await response.Content.ReadFromJsonAsync<PersonResponse>();
        personResponse.Should().BeEquivalentTo(person);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_ReturnsValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        var person = PersonGenerator.Generate();
        var personId = Guid.NewGuid();

        var createdResponse = await Client.PostAsJsonAsync($"people/{personId}", person);
        var createdPerson = await createdResponse.Content.ReadFromJsonAsync<PersonResponse>();

        const string invalidEmail = "someInvalidEmail";
        person = PersonGenerator.Clone()
            .RuleFor(x => x.Email, invalidEmail).Generate();

        // Act
        var response = await Client.PutAsJsonAsync($"people/{createdPerson!.Id}", person);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error.Errors.Should().ContainKey("Person.Email");
    }
}
