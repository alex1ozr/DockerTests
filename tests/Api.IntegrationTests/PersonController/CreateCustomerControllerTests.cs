using System.Net;
using System.Net.Http.Json;
using Bogus;
using DockerTestsSample.Api.Contracts.Requests;
using DockerTestsSample.Api.Contracts.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.PersonController;

[Collection("Test collection")]
public class CreatePersonControllerTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;

    private readonly Faker<PersonRequest> _customerGenerator = new Faker<PersonRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.Name, faker => faker.Person.FirstName)
        .RuleFor(x => x.LastName, faker => faker.Person.LastName)
        .RuleFor(x => x.BirthDate, faker => faker.Person.DateOfBirth.Date);

    public CreatePersonControllerTests(PersonApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;
        _resetDatabase = apiFactory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task Create_CreatesUser_WhenDataIsValid()
    {
        // Arrange
        var personRequest = _customerGenerator.Generate();
        var personId = Guid.NewGuid();

        // Act
        var response = await _client.PostAsJsonAsync($"people/{personId}", personRequest);

        // Assert
        var customerResponse = await response.Content.ReadFromJsonAsync<PersonResponse>();
        customerResponse.Should().BeEquivalentTo(personRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should()
            .Be($"http://localhost/people/{customerResponse!.Id}");
    }

    [Fact]
    public async Task Create_ReturnsValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        const string invalidEmail = "wgwegw";
        var personRequest = _customerGenerator.Clone()
            .RuleFor(x => x.Email, invalidEmail).Generate();

        // Act
        var response = await _client.PostAsJsonAsync($"people/{Guid.NewGuid()}", personRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error.Errors.ContainsKey("Person.Email");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
