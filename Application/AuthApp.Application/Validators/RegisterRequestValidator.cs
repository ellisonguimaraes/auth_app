using AuthApp.Application.Models;
using AuthApp.Infra.CrossCutting.Resources;
using FluentValidation;

namespace AuthApp.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(ar => ar.Email)
            .EmailAddress().WithMessage(ErrorCodeResource.VALIDATION_INVALID_PROPERTY);

        RuleFor(ar => ar.Name)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .NotEmpty().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_EMPTY);

        RuleFor(ar => ar.Password)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .NotEmpty().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_EMPTY);

        RuleFor(ar => ar.PasswordRepeat)
            .Equal(ar => ar.Password).WithMessage(ErrorCodeResource.VALIDATION_NOT_EQUAL_PASSWORD);

        RuleFor(ar => ar.Document)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .NotEmpty().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_EMPTY);

        RuleFor(ar => ar.DocumentType)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .IsInEnum().WithMessage(ErrorCodeResource.VALIDATION_INVALID_PROPERTY);

        RuleFor(ar => ar.UserType)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .IsInEnum().WithMessage(ErrorCodeResource.VALIDATION_INVALID_PROPERTY);
    }
}
