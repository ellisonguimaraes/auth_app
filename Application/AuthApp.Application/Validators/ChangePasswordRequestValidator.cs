using AuthApp.Application.Models;
using AuthApp.Infra.CrossCutting.Resources;
using FluentValidation;

namespace AuthApp.Application.Validators;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(ar => ar.Password)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .NotEmpty().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_EMPTY);

        RuleFor(ar => ar.NewPassword)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .NotEmpty().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_EMPTY);

        RuleFor(ar => ar.NewPasswordRepeat)
            .Equal(ar => ar.NewPassword).WithMessage(ErrorCodeResource.VALIDATION_NOT_EQUAL_PASSWORD);
    }
}
