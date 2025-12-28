namespace Users.Infrastructure.Authentication;

public sealed class ServiceIdentitiesSettings
{
    public const string SectionName = "ServiceIdentitySettings";
    public ServiceIdentitySettings[] ServiceIdentities { get; set; } = [];
}

public sealed class ServiceIdentitySettings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Scopes { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}