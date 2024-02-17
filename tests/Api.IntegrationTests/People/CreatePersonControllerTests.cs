using DockerTestsSample.Api.IntegrationTests.Abstract;
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
        Func<Task> f = async () => await Client.People.CreatePersonAsync(Guid.NewGuid(), personRequest);
        var exception = await f.Should().ThrowAsync<ApiException<ValidationProblemDetails>>();
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
        Func<Task> f = async () => await Client.People.CreatePersonAsync(personId, personRequest);
        var exception = await f.Should().ThrowAsync<ApiException<ProblemDetails>>();
        exception.Which.Result.Should()
            .BeEquivalentTo(new
            {
                Type = "person_already_exists",
                Status = StatusCodes.Status409Conflict,
            });
    }
}
