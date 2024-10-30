using DockerTestsSample.Api.Contracts.People;
using DockerTestsSample.Api.Validation;
using FluentAssertions;
using Xunit;

namespace DockerTestsSample.UnitTests.Validators;

public sealed class PersonRequestValidatorTests
{
    private readonly PersonRequestValidator _sut = new();

    [Fact]
    public void Validate_WhenPersonRequestIsValid_ShouldReturnSuccess()
    {
        // Act
        var result = _sut.Validate(GetPersonRequest());

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenNameIsEmpty_ShouldReturnFailure()
    {
        // Act
        var result = _sut.Validate(GetPersonRequest(name: string.Empty));

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().OnlyContain(x => x.PropertyName == nameof(PersonRequest.Name));
    }

    [Fact]
    public void Validate_WhenLastNameIsEmpty_ShouldReturnFailure()
    {
        // Act
        var result = _sut.Validate(GetPersonRequest(lastName: string.Empty));

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().OnlyContain(x => x.PropertyName == nameof(PersonRequest.LastName));
    }

    [Fact]
    public void Validate_WhenEmailIsEmpty_ShouldReturnFailure()
    {
        // Act
        var result = _sut.Validate(GetPersonRequest(email: "incorrectEmail"));

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().OnlyContain(x => x.PropertyName == nameof(PersonRequest.Email));
    }

    private PersonRequest GetPersonRequest(
        string name = "someName",
        string lastName = "someLastName",
        string email = "some@mail.com")
        => new()
        {
            Name = name,
            LastName = lastName,
            BirthDate = new DateOnly(2020, 12, 12),
            Email = email,
        };
}