using ErrorOr;
using MediatR;
using Users.Application.Auth.Common;
using Users.Application.Common.Abstractions.Repositories;
using Users.Application.Common.Abstractions.Services;
using Users.Domain;

namespace Users.Application.Auth.Command.RegisterUser;

public sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<RegisterUserCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

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

        string token = _jwtTokenGenerator.GenerateToken(user);
        _userRepository.Create(user);
        AuthenticationResult result = new(user.Id, command.Email, token);
        return result;
    }
}