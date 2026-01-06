using Users.Domain.Enums;
using Users.Infrastructure.Authorization;

namespace Users.Application.Common.Abstractions.Authorization;

public interface IPermissionProvider
{
    IReadOnlyCollection<string> GetPermissions(ClientType entityType);
}