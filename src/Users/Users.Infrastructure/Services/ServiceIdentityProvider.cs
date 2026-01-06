using Microsoft.Extensions.Options;
using Users.Application.Common.Abstractions.Services;
using Users.Infrastructure.Security;

namespace Users.Infrastructure.Services;

public sealed class ServiceIdentityProvider(
    IOptions<ServicesSettings> options)
    : IServiceIdentityProvider
{
    private readonly ServicesSettings _servicesSettings = options.Value;

    public ServiceIdentity? GetService(string name)
    {
        Service? service = _servicesSettings
            .Services
            .FirstOrDefault(s => s.Name.Equals(name));

        if (service is null)
        {
            return null;
        }

        ServiceIdentity serviceIdentity = new()
        {
            Id = Guid.NewGuid(),
            Name = service.Name,
            Secret = service.Secret,
            ClientType = Domain.Enums.ClientType.Service,
        };

        return serviceIdentity;
    }
}