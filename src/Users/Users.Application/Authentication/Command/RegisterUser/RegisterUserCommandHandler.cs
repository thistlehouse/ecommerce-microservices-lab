using ErrorOr;
using MediatR;
using Users.Application.Authentication.Common;
using Users.Application.Common.Abstractions.Repositories;
using Users.Application.Common.Abstractions.Services;
using Users.Application.Common.Abstractions.Services.EmailNotifications;
using Users.Domain;

namespace Users.Application.Authentication.Command.RegisterUser;

public sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IEmailNotification emailNotification)
    : IRequestHandler<RegisterUserCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IEmailNotification _emailNotification = emailNotification;

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        User? user = _userRepository.GetByEmail(command.Email);
        if (user is not null)
        {
            return Error.Conflict("EmailExists", "Email is already registered");
        }

        user = User.Create(
            command.FirstName,
            command.LastName,
            command.Email,
            command.Password);

        // TODO: hash password

        string token = _jwtTokenGenerator.GenerateUserToken(user);
        _userRepository.Create(user);

        Message message = MessageBuilder.New()
            .WithSmtpHost("localhost", 1025)
            .FromEmail(user.Email)
            .ToEmail("no-reply@ecommerce.com")
            .WithSubject("Email Confirmation")
            .WithBody($@"
                <html>
                <body>
                    <p>Please confirm your email. Click the link below:</p>
                    <p><a href='http://localhost:5099/email/confirmation?email={user.Email}'>Confirm Email</a></p>
                </body>
                </html>")
            .BuildEmailConfirmationMessage();

        _emailNotification.SendNotification(message);
        AuthenticationResult result = new(user.Id, command.Email, token);
        return result;
    }
}