/*
using DockerTestsSample.Api.Contracts.Requests;
using FluentValidation;
using JetBrains.Annotations;

namespace DockerTestsSample.Api.Validation;

[UsedImplicitly]
public sealed class UpdatePersonRequestValidator : AbstractValidator<PersonRequest>
{
    public UpdatePersonRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.BirthDate).NotEmpty();
    }
}
*/
