using AuthApp.Infra.CrossCutting.Resources;
using FluentValidation;

namespace AuthApp.Application;

public class ContactEmailRequestValidator : AbstractValidator<ContactEmailRequest>
{
    public ContactEmailRequestValidator()
    {
        RuleFor(c => c.Email)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .NotEmpty().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_EMPTY);

        RuleFor(c => c.Message)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .NotEmpty().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_EMPTY);

        RuleFor(c => c.Subject)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .NotEmpty().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_EMPTY);

        RuleFor(c => c.Name)
            .NotNull().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_NULL)
            .NotEmpty().WithMessage(ErrorCodeResource.VALIDATION_CANNOT_BE_EMPTY);
    }
}
