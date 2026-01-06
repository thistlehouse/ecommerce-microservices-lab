namespace Users.Infrastructure.Security;

public sealed class ServicesSettings
{
    public const string SectionName = "ServicesSettings";
    public Service[] Services { get; set; } = [];
}

public sealed class Service
{
    public string Name { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}