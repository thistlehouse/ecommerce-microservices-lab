namespace Users.Contracts.Auth;

public sealed record AuthenticationResponse(Guid Id, string Email, string Token);