using DockerTestsSample.Api.Contracts.People;
using FluentValidation;

namespace DockerTestsSample.Api.Validation;

public sealed class PersonRequestValidator : AbstractValidator<PersonRequest>
{
    public PersonRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.BirthDate).NotEmpty();
    }
}
