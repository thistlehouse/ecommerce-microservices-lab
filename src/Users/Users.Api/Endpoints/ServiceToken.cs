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
            async (ISender sender, ServiceTokenRequest request) =>
        {
            ServiceTokenCommand command = new(request.ClientId, request.ClientSecret, request.Scopes);
            ErrorOr<string> result = await sender.Send(command, default);

            return result.Match(
                token => Results.Ok(token),
                errors => ApiErrors.Problem(errors));
        });
    }

    private sealed record ServiceTokenRequest(
        [property: JsonPropertyName(name: "client_id")] string ClientId,
        [property: JsonPropertyName(name: "client_secret")] string ClientSecret,
        string Scopes);
}