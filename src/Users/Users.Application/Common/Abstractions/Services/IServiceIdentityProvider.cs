namespace Users.Application.Common.Abstractions.Services;

public interface IServiceIdentityProvider
{
    ServiceIdentity? GetService(string name);
}