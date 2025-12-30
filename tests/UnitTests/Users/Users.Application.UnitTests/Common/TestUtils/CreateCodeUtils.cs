using Users.Domain;
using Users.Domain.Enums;

namespace Users.Application.UnitTests.Common.TestUtils;

public static class CreateCodeUtils
{
    public static Code CreateCode(
        Guid? userId = null,
        string? value = null,
        CodePurpose purpose = CodePurpose.EmailConfirmation,
        int expiresInMinutes = 5) =>
        Code.Create(
            userId ?? Guid.NewGuid(),
            value ?? "123456",
            purpose,
            expiresInMinutes);
}