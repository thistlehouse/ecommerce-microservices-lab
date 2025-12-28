using ErrorOr;
using MediatR;

namespace Users.Application.Authentication.Command.ServiceToken;


public sealed record ServiceTokenCommand(
    string ClientId,
    string ClientSecret,
    string Scopes) : IRequest<ErrorOr<string>>;