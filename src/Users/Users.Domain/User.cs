using Users.Domain.Enums;

namespace Users.Domain;

public class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public bool IsEmailVerified { get; private set; }
    public string Password { get; private set; } = string.Empty;
    public UserType UserType { get; private set; } = UserType.Customer;

    private User(Guid id, string firstName, string lastName, string email, string password)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }

    public static User Create(string firstName, string lastName, string email, string password)
    {
        return new(Guid.NewGuid(), firstName, lastName, email, password);
    }

    public void ConfirmEmailVerification()
    {
        IsEmailVerified = true;
    }
}
