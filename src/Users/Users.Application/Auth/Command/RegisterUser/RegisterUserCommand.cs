using ErrorOr;
using MediatR;
using Users.Application.Auth.Common;

namespace Users.Application.Auth.Command.RegisterUser;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;