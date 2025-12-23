using ErrorOr;
using MediatR;
using Users.Api.Errors;
using Users.Application.Authentication.Command.RegisterUser;
using Users.Application.Authentication.Common;

namespace Users.Api.Endpoints;

public sealed class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/register",
            async (ISender sender, RegisterUserRequest request) =>
        {
            RegisterUserCommand command = new(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password);

            ErrorOr<AuthenticationResult> result = await sender.Send(command, default);

            return result.Match(
                auth => Results.Created("/register", auth),
                errors => ApiErrors.Problem(errors));
        });
    }

    private record RegisterUserRequest(
        string FirstName,
        string LastName,
        string Email,
        string Password);
}