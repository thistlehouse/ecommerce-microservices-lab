namespace Users.Application.Common.Abstractions.Services;

public sealed class ServiceIdentity
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Scopes { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;

    public bool IsSecretValid(string secret) => ClientSecret.Equals(secret);
}
