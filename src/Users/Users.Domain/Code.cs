using Users.Domain.Enums;

namespace Users.Domain;

public sealed class Code
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Value { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? UsedAt { get; private set; }
    public CodePurpose Purpose { get; private set; }

    private Code(Guid id, Guid userId, string value, CodePurpose purpose, int expiresInMinutes)
    {
        Id = id;
        UserId = userId;
        Value = value;
        Purpose = purpose;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = CreatedAt.AddMinutes(expiresInMinutes);
    }

    public static Code Create(
        Guid userId,
        string value,
        CodePurpose purpose,
        int expiresInMinutes = 5) =>
        new(
            Guid.NewGuid(),
            userId,
            value,
            purpose,
            expiresInMinutes);

    public void MarkAsUsed(DateTime utcNow)
    {
        if (UsedAt is not null) throw new InvalidOperationException("Code already used");

        UsedAt = utcNow;
    }
}