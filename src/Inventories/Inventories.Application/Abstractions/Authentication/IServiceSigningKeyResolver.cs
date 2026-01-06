using Microsoft.IdentityModel.Tokens;

namespace Inventories.Infrastructure.Authentication;

public interface IServiceSigningKeyResolver
{
    SecurityKey ResolveKey(string clientId);

}