using Users.Domain;

namespace Users.Application.Common.Abstractions.Repositories;

public interface IUserRepository
{
    void Add(User user);
    User? GetByEmail(string email);
    void ConfirmEmail(string email);
}