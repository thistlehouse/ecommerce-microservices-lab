using System.Text;

namespace Users.Domain.Permissions;

public struct JwtPermissionClaimNames
{
    public const string UserType = "user_type";
    public const string Permissions = "permissions";
}