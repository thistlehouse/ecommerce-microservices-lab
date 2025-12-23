using ErrorOr;
using MediatR;
using Users.Application.Authentication.Common;

namespace Users.Application.Authentication.Command.RegisterUser;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;