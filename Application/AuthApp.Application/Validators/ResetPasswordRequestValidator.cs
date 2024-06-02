using AuthApp.Application.Models;
using AuthApp.Infra.CrossCutting.Resources;
using FluentValidation;

namespace AuthApp.Application.Validators;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(rp => rp.Email)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .NotEmpty().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_EMPTY)
            .EmailAddress().WithMessage(ErrorCodeResource.VALIDATION_INVALID_PROPERTY);

        RuleFor(rp => rp.NewPassword)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .NotEmpty().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_EMPTY);

        RuleFor(rp => rp.Token)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .NotEmpty().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_EMPTY);
    }
}
