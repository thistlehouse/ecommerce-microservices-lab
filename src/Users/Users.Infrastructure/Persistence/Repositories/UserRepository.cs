using Users.Application.Common.Abstractions.Repositories;
using Users.Domain;

namespace Users.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private static List<User> _users = [];

    public void Add(User user)
    {
        _users.Add(user);
    }

    public User? GetByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email.Equals(email));
    }

    public User? GetById(Guid userId)
    {
        return _users.FirstOrDefault(u => u.Id == userId);
    }

    public void PatchUser(User user)
    {
        int idx = _users.IndexOf(user);
        _users[idx] = user;
    }
}