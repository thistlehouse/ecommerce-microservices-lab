using ErrorOr;
using MediatR;
using Users.Api.Errors;
using Users.Application.Authentication.Command.EmailConfirmation;
using Users.Application.Common.Abstractions.Repositories;
using Users.Domain;

namespace Users.Api.Endpoints;

public sealed class EmailConfirmation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "email/confirmation",
            async (ISender sender, ConfirmationRequest request) =>
        {
            EmailConfirmationCommand command = new(request.ConfirmationCode);
            ErrorOr<Unit> result = await sender.Send(command, default);

            return result.Match(
                _ => Results.Ok(),
                errors => ApiErrors.Problem(errors));
        });
    }

    private sealed record ConfirmationRequest(string ConfirmationCode);
}