namespace Users.Application.Auth.Common;

public sealed record AuthenticationResult(
    Guid Id,
    string Email,
    string Token);