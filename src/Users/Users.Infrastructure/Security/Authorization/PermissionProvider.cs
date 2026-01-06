using System.ComponentModel;
using Microsoft.Extensions.Options;
using Users.Application.Common.Abstractions.Authorization;
using Users.Domain.Enums;

namespace Users.Infrastructure.Authorization;

public sealed class PermissionProvider(IOptions<PermissionSettings> options)
    : IPermissionProvider
{
    private readonly PermissionSettings _permissionSettings = options.Value;

    public IReadOnlyCollection<string> GetPermissions(ClientType entityType)
    {
        return entityType switch
        {
            ClientType.Customer => _permissionSettings.CustomerPermissions,
            ClientType.Admin => _permissionSettings.AdminPermissions,
            ClientType.Service => _permissionSettings.ProductServicePermissions,
            _ => throw new InvalidEnumArgumentException($"There is no type with the {nameof(entityType)}"),
        };
    }
}