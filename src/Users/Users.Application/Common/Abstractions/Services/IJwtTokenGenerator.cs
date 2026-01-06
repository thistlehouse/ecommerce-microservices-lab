using Users.Domain;

namespace Users.Application.Common.Abstractions.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(Entity entity);
}