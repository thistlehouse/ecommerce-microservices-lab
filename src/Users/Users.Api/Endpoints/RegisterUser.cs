namespace Users.Api.Endpoints;

public sealed class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/register",
            (RegisterUserRequest request) => $"Hello {request.FirstName}");
    }

    private record RegisterUserRequest(
        string FirstName,
        string LastName,
        string Email,
        string Password);
}