namespace Products.Infrastructure.Persistence.Repositories;

public sealed class CacheSettings
{
    public int ExpirationMinutes { get; init; } = 10;
}