using FluentValidation;

namespace Users.Application.Authentication.Command.ResendEmailConfirmation;

public class ResendEmailConfirmationCommandValidator : AbstractValidator<ResendEmailConfirmationCommand>
{
    public ResendEmailConfirmationCommandValidator()
    {
        RuleFor(r => r.UserEmail)
            .NotNull()
            .NotEmpty()
            .WithMessage("Value should not be null or empty")
            .EmailAddress()
            .WithMessage("Email should be valid");
    }
}