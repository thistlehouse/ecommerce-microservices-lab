using ErrorOr;
using MediatR;
using Users.Application.Authentication.Common;
using Users.Application.Common.Abstractions.Repositories;
using Users.Application.Common.Abstractions.Services;
using Users.Application.Common.Abstractions.Services.ConfirmationCodes;
using Users.Application.Common.Abstractions.Services.EmailNotifications;
using Users.Domain;
using Users.Domain.Enums;

namespace Users.Application.Authentication.Command.RegisterUser;

public sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IEmailNotification emailNotification,
    IConfirmationCodeGenerator confirmationCodeGenerator,
    ICodeRepository codeRepository)
    : IRequestHandler<RegisterUserCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IEmailNotification _emailNotification = emailNotification;
    private readonly IConfirmationCodeGenerator _confirmationCodeGenerator = confirmationCodeGenerator;
    private readonly ICodeRepository _codeRepository = codeRepository;

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
            command.Password,
            ClientType.Customer);

        // TODO: hash password

        string token = _jwtTokenGenerator.GenerateToken(user);
        string value = _confirmationCodeGenerator.GenerateConfirmationCode(6);
        Code code = Code.Create(user.Id, value, Domain.Enums.CodePurpose.EmailConfirmation);

        Message message = MessageBuilder.New()
            .WithSmtpHost("172.18.16.251", 1025)
            .FromEmail(user.Email)
            .ToEmail("no-reply@ecommerce.com")
            .WithSubject("Email Confirmation")
            .WithCode(value)
            .WithBody("Please, use the code bellow to confirm your email")
            .BuildEmailConfirmationMessage();

        _userRepository.Add(user);
        _codeRepository.Add(code);

        // TODO: retry
        try
        {
            await _emailNotification.SendNotificationAsync(message);
        }
        catch (Exception)
        {
            // TODO: Log failure
        }

        AuthenticationResult result = new(user.Id, command.Email, token);
        return result;
    }
}