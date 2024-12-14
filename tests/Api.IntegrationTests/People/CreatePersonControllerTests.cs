using DockerTestsSample.Api.IntegrationTests.Infrastructure;
using DockerTestsSample.Client.Implementations;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.People;

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

        // Act, Assert
        var exception = await Client.People
            .Invoking(p => p.CreatePersonAsync(Guid.NewGuid(), personRequest))
            .Should().ThrowAsync<ApiException<ValidationProblemDetails>>();

        var problemDetails = exception.Which.Result;
        problemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Errors.Should().ContainKey("Email");
    }

    [Fact]
    public async Task Create_ReturnsError_WhenPersonAlreadyExists()
    {
        // Arrange
        var personRequest = PersonGenerator.Generate();
        var personId = Guid.NewGuid();
        await Client.People.CreatePersonAsync(personId, personRequest);

        // Act, Assert
        var exception = await Client.People
            .Invoking(p => p.CreatePersonAsync(personId, personRequest))
            .Should().ThrowAsync<ApiException<ProblemDetails>>();

        exception.Which.Result.Should()
            .BeEquivalentTo(new
            {
                Type = "person_already_exists",
                Status = StatusCodes.Status409Conflict,
            });
    }
}