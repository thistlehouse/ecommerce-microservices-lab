using ErrorOr;
using MediatR;

namespace Users.Application.Authentication.Command.ServiceToken;


public sealed record ServiceTokenCommand(
    string ServiceName,
    string Secret) : IRequest<ErrorOr<string>>;