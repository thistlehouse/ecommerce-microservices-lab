using ErrorOr;
using MediatR;
using Users.Application.Common.Abstractions.Repositories;
using Users.Domain;

namespace Users.Application.Authentication.Command.EmailConfirmation;

public sealed class EmailConfirmationCommandHandler(
    ICodeRepository codeRepository,
    IUserRepository userRepository)
    : IRequestHandler<EmailConfirmationCommand, ErrorOr<Unit>>
{
    private readonly ICodeRepository _codeRepository = codeRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ErrorOr<Unit>> Handle(
        EmailConfirmationCommand command,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;


        if (_codeRepository.Get(command.Code) is not Code code)
        {
            return Error.Validation("InvalidCode", "Code is invalid");
        }

        if (code.UsedAt is not null || code.ExpiresAt < code.CreatedAt)
        {
            return Error.Validation("CodeExpired", "This code has expired");
        }

        if (_userRepository.GetById(code.UserId) is not User user)
        {
            return Error.NotFound("UserNotFound", "User was not found");
        }

        code.MarkAsUsed(DateTime.UtcNow);
        user.MarkEmailAsVerified();
        _userRepository.PatchUser(user);

        return new Unit();
    }
}