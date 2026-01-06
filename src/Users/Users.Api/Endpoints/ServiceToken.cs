using System.Text.Json.Serialization;
using ErrorOr;
using MediatR;
using Users.Api.Errors;
using Users.Application.Authentication.Command.ServiceToken;

namespace Users.Api.Endpoints;

public sealed class ServiceToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/token",
            async (ISender sender, ServiceRequest request) =>
        {
            ServiceTokenCommand command = new(request.ServiceName, request.Secret);
            ErrorOr<string> result = await sender.Send(command, default);

            return result.Match(
                token => Results.Ok(new { token }),
                errors => ApiErrors.Problem(errors));
        });
    }

    private sealed record ServiceRequest(
        string ServiceName,
        string Secret);
}