using Microsoft.Extensions.Options;
using Users.Application.Common.Abstractions.Services;
using Users.Infrastructure.Authentication;

namespace Users.Infrastructure.Services;

public sealed class ServiceIdentityProvider(
    IOptions<ServiceIdentitiesSettings> serviceIdentitiesSettings)
    : IServiceIdentityProvider
{
    private readonly ServiceIdentitiesSettings _serviceIdentitiesSettings = serviceIdentitiesSettings.Value;

    public ServiceIdentity? GetByClientId(string clientId)
    {
        ServiceIdentitySettings? serviceIdentitySettings = _serviceIdentitiesSettings
            .ServiceIdentities
            .FirstOrDefault(s => s.ClientId.Equals(clientId));

        if (serviceIdentitySettings is null)
        {
            return null;
        }

        ServiceIdentity serviceIdentity = new()
        {
            ClientId = serviceIdentitySettings!.ClientId,
            ClientSecret = serviceIdentitySettings.ClientSecret,
            Scopes = serviceIdentitySettings.Scopes,
        };

        return serviceIdentity;
    }
}