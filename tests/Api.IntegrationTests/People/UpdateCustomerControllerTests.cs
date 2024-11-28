using DockerTestsSample.Api.IntegrationTests.Infrastructure;
using DockerTestsSample.Client.Implementations;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.People;

public sealed class UpdatePersonControllerTests : ControllerTestsBase
{
    public UpdatePersonControllerTests(TestApplication apiFactory) :
        base(apiFactory)
    {
    }

    [Fact]
    public async Task Update_UpdatesUser_WhenDataIsValid()
    {
        // Arrange
        var personRequest = PersonGenerator.Generate();
        var personId = Guid.NewGuid();

        await Client.People.CreatePersonAsync(personId, personRequest);

        personRequest = PersonGenerator.Generate();

        // Act
        var response = await Client.People.UpdatePersonAsync(personId, personRequest);

        // Assert
        response.Should().BeEquivalentTo(personRequest);
    }

    [Fact]
    public async Task Update_ReturnsValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        var personRequest = PersonGenerator.Generate();
        var personId = Guid.NewGuid();

        var createdPerson = await Client.People.CreatePersonAsync(personId, personRequest);

        const string invalidEmail = "someInvalidEmail";
        personRequest = PersonGenerator.Clone()
            .RuleFor(x => x.Email, invalidEmail).Generate();

        // Act, Assert
        Func<Task> f = async () => await Client.People.UpdatePersonAsync(createdPerson!.Id, personRequest);
        var exception = await f.Should().ThrowAsync<ApiException<ValidationProblemDetails>>();
        var problemDetails = exception.Which.Result;
        problemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Errors.Should().ContainKey("Email");
    }

    [Fact]
    public async Task Update_ReturnsError_WhenPersonDoesNotExist()
    {
        // Arrange
        var personRequest = PersonGenerator.Generate();

        // Act, Assert
        Func<Task> f = async () => await Client.People.UpdatePersonAsync(Guid.NewGuid(), personRequest);
        var exception = await f.Should().ThrowAsync<ApiException<ProblemDetails>>();
        exception.Which.Result.Should()
            .BeEquivalentTo(new
            {
                Type = "person_not_found",
                Status = StatusCodes.Status404NotFound,
            });
    }
}
