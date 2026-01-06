using ErrorOr;
using MediatR;
using Users.Application.Common.Abstractions.Services;

namespace Users.Application.Authentication.Command.ServiceToken;

public sealed class ServiceTokenCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IServiceIdentityProvider serviceIdentityProvider)
    : IRequestHandler<ServiceTokenCommand, ErrorOr<string>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IServiceIdentityProvider _serviceIdentityProvider = serviceIdentityProvider;

    public async Task<ErrorOr<string>> Handle(ServiceTokenCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        ServiceIdentity? serviceIdentity = _serviceIdentityProvider.GetService(command.ServiceName);
        if (serviceIdentity is null)
        {
            return Error.Unauthorized("Service does not exist");
        }

        if (!serviceIdentity.IsSecretValid(command.Secret))
        {
            return Error.Unauthorized("Invalid Client Credentials");
        }

        string token = _jwtTokenGenerator.GenerateToken(serviceIdentity);
        return token;
    }
}