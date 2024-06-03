using AuthApp.Application.Models;
using AuthApp.Infra.CrossCutting.Resources;
using FluentValidation;

namespace AuthApp.Application.Validators;

public class AuthRequestValidator : AbstractValidator<AuthRequest>
{
    public AuthRequestValidator()
    {
        RuleFor(ar => ar.Email)
            .EmailAddress().WithMessage(ErrorCodeResource.VALIDATION_INVALID_PROPERTY);

        RuleFor(ar => ar.Password)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .NotEmpty().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_EMPTY);
    }
}
