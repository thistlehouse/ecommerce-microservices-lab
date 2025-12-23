namespace Users.Application.Authentication.Common;

public sealed record AuthenticationResult(
    Guid Id,
    string Email,
    string Token);