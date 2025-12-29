namespace Users.Application.Common.Abstractions.Services.ConfirmationCodes;

public interface IConfirmationCodeGenerator
{
    string GenerateConfirmationCode(double length);
}