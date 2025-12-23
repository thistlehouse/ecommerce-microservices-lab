using ErrorOr;
using MediatR;
using Users.Application.Authentication.Common;

namespace Users.Application.Authentication.Queries;

public sealed record LoginUserQuery(string Email, string Password) : IRequest<ErrorOr<AuthenticationResult>>;