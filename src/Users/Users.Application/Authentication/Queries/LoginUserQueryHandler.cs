using ErrorOr;
using MediatR;
using Users.Application.Authentication.Common;
using Users.Application.Common.Abstractions.Repositories;
using Users.Application.Common.Abstractions.Services;
using Users.Domain;

namespace Users.Application.Authentication.Queries;

public sealed class LoginUserQueryHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<LoginUserQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        LoginUserQuery query,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (_userRepository.GetByEmail(query.Email) is not User user)
        {
            return Error.NotFound("UserNotFound", "User was not found.");
        }

        if (!user.IsEmailVerified)
        {
            return Error.Failure("EmailNotConfirmed", "Email was not yet confirmed");
        }

        if (user.Password != query.Password)
        {
            return Error.Validation("InvalidCredentials", "Credentials are not valid");
        }

        string token = _jwtTokenGenerator.GenerateToken(user);
        AuthenticationResult result = new(user.Id, user.Email, token);
        return result;
    }
}