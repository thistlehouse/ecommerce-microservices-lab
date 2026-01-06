using Users.Domain;
using Users.Domain.Enums;

namespace Users.Application.UnitTests.Common.TestUtils;

public static class CreateUserUtils
{
    public static User CreateUser(
        string firstName = "FirstName",
        string lastName = "LastName",
        string email = "user@mail.com",
        string password = "password",
        ClientType clientType = ClientType.Customer) =>
        User.Create(firstName, lastName, email, password, clientType);
}