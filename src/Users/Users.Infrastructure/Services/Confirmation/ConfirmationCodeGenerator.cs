using System.Security.Cryptography;
using Users.Application.Common.Abstractions.Services.ConfirmationCodes;

namespace Users.Infrastructure.Services.Confirmation;

public sealed class ConfirmationCodeGenerator : IConfirmationCodeGenerator
{
    public string GenerateConfirmationCode(double length)
    {
        int max = (int)Math.Pow(10, length);
        int code = RandomNumberGenerator.GetInt32(0, max);
        return code.ToString($"D{length}");
    }
}