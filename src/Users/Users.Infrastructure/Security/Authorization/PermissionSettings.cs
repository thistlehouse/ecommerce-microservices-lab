namespace Users.Infrastructure.Authorization;

public sealed class PermissionSettings
{
    public const string SectionName = "Permissions";
    public string[] CustomerPermissions { get; set; } = [];
    public string[] AdminPermissions { get; set; } = [];
    public string[] ProductServicePermissions { get; set; } = [];
}