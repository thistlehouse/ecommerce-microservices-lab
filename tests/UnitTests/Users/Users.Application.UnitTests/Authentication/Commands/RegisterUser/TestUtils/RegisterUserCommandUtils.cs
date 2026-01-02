using Users.Application.Authentication.Command.RegisterUser;

namespace Users.Application.UnitTests.Authentication.Commands.RegisterUser.TestUtils;

public static class RegisterUserCommandUtils
{
    public static RegisterUserCommand CreateRegisterCommand(
        string username = "UserFistName",
        string lastName = "UserLastName",
        string email = "user@mail.com",
        string password = "password") =>
        new(
            username,
            lastName,
            email,
            password);

    public static string GenerateToken() => "token.token.token";

    public static string GenerateCode() => "123456";
}