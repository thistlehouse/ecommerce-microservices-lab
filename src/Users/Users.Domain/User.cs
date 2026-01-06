using Users.Domain.Enums;

namespace Users.Domain;

public class User : Entity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public bool IsEmailVerified { get; private set; }
    public string Password { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private User(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string password,
        ClientType clientType = ClientType.Admin)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        ClientType = ClientType.Admin;
        CreatedAt = DateTime.UtcNow;
    }

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string password,
        ClientType clientType)
    {
        return new(
            Guid.NewGuid(),
            firstName,
            lastName,
            email,
            password,
            clientType);
    }

    public void MarkEmailAsVerified()
    {
        IsEmailVerified = true;
    }
}
