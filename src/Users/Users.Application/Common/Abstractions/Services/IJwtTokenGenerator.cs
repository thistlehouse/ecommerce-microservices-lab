using Users.Domain;

namespace Users.Application.Common.Abstractions.Services;

public interface IJwtTokenGenerator
{
    string GenerateUserToken(User user);
    string GenerateServiceToken(ServiceIdentity serviceIdentity);
}