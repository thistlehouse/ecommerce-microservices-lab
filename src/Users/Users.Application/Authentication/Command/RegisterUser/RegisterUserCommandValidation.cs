using FluentValidation;

namespace Users.Application.Authentication.Command.RegisterUser;

public sealed class RegisterUserCommandValidation : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidation()
    {
        RuleFor(r => r.FirstName)
            .NotNull()
            .NotEmpty()
            .WithMessage("FirstName cannot be null or empty.")
            .MinimumLength(3)
            .WithMessage("FirstName min length is 3")
            .MaximumLength(15)
            .WithMessage("FirstName max length is 15");

        RuleFor(r => r.LastName)
            .NotNull()
            .NotEmpty()
            .WithMessage("LastName cannot be null or empty.")
            .MinimumLength(3)
            .WithMessage("LastName min length is 3")
            .MaximumLength(15)
            .WithMessage("LastName max length is 15");

        RuleFor(r => r.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Email cannot be null or empty.")
            .EmailAddress()
            .WithMessage("Email has to be a valid email address.");

        RuleFor(r => r.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password cannot be null or empty.")
            .MinimumLength(3)
            .WithMessage("Password min length is 8")
            .MaximumLength(15)
            .WithMessage("LastName max length is 12");
    }
}