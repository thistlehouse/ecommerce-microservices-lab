using Users.Domain;

namespace Users.Application.Common.Abstractions.Services;

public sealed class ServiceIdentity : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;

    public bool IsSecretValid(string value) => Secret.Equals(value);
}
