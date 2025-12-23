using ErrorOr;
using MediatR;
using Users.Api.Errors;
using Users.Application.Authentication.Common;
using Users.Application.Authentication.Queries;

namespace Users.Api.Endpoints;

public sealed class LoginUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/login",
            async (ISender sender, LoginRequest request) =>
        {
            LoginUserQuery query = new(request.Email, request.Password);
            ErrorOr<AuthenticationResult> result = await sender.Send(query, default);

            return result.Match(
                auth => Results.Ok(auth),
                errors => ApiErrors.Problem(errors));
        });
    }

    private sealed record LoginRequest(string Email, string Password);
}