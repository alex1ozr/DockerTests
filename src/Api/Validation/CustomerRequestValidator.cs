using DockerTestsSample.Api.Contracts.Requests;
using FluentValidation;

namespace DockerTestsSample.Api.Validation;

public sealed class CustomerRequestValidator : AbstractValidator<PersonRequest>
{
    public CustomerRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.BirthDate).NotEmpty();
    }
}
