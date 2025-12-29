using Users.Application.Common.Abstractions.Repositories;
using Users.Domain;

namespace Users.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private static List<User> _users = [];

    public void ConfirmEmail(string email)
    {
        User? user = _users.FirstOrDefault(u => u.Email.Equals(email));
        user?.ConfirmEmailVerification();
    }

    public void Create(User user)
    {
        _users.Add(user);
    }

    public User? GetByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email.Equals(email));
    }
}