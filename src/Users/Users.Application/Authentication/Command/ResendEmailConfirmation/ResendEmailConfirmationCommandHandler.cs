using ErrorOr;
using MediatR;
using Users.Application.Common.Abstractions.Repositories;
using Users.Application.Common.Abstractions.Services.ConfirmationCodes;
using Users.Application.Common.Abstractions.Services.EmailNotifications;
using Users.Domain;

namespace Users.Application.Authentication.Command.ResendEmailConfirmation;

public sealed class ResendEmailConfirmationCommandHandler(
    ICodeRepository codeRepository,
    IUserRepository userRepository,
    IEmailNotification emailNotification,
    IConfirmationCodeGenerator codeGenerator)
    : IRequestHandler<ResendEmailConfirmationCommand, ErrorOr<Unit>>
{
    private readonly ICodeRepository _codeRepository = codeRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmailNotification _emailNotification = emailNotification;
    private readonly IConfirmationCodeGenerator _codeGenerator = codeGenerator;

    public async Task<ErrorOr<Unit>> Handle(
        ResendEmailConfirmationCommand command,
        CancellationToken cancellationToken)

    {
        await Task.CompletedTask;

        User? user = _userRepository.GetByEmail(command.UserEmail);
        if (user is null)
        {
            return Error.NotFound("UserNotFound", "User was not found");
        }

        Code? code = _codeRepository.GetByUserId(user.Id);
        if (code is null)
        {
            return Error.Validation("InvalidCode", "Code is invalid");
        }

        string newCode = _codeGenerator.GenerateConfirmationCode(6);
        code.RenewCode(newCode);
        _codeRepository.Patch(code);
        Message message = CreateMessage(user.Email, code.Value);

        try
        {
            await _emailNotification.SendNotificationAsync(message);
        }
        catch (Exception)
        {
            // TODO: Log failure
        }

        return new Unit();
    }

    private Message CreateMessage(string userEmail, string code)
    {
        Message message = MessageBuilder.New()
            .WithSmtpHost("localhost", 1025)
            .FromEmail(userEmail)
            .ToEmail("no-reply@ecommerce.com")
            .WithSubject("Email Confirmation")
            .WithCode(code)
            .WithBody("Please, use the code bellow to confirm your email")
            .BuildEmailConfirmationMessage();

        return message;
    }
}