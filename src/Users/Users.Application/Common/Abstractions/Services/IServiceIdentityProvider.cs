namespace Users.Application.Common.Abstractions.Services;

public interface IServiceIdentityProvider
{
    ServiceIdentity? GetByClientId(string clientId);
}