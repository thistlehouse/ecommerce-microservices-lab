using Users.Domain;

namespace Users.Application.Common.Abstractions.Repositories;

public interface IUserRepository
{
    void Create(User user);
    User? GetByEmail(string email);

}