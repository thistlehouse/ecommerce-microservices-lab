using ErrorOr;
using MediatR;
using Users.Api.Errors;
using Users.Application.Authentication.Command.ResendEmailConfirmation;
using Users.Application.Common.Abstractions.Repositories;
using Users.Application.Common.Abstractions.Services.ConfirmationCodes;
using Users.Application.Common.Abstractions.Services.EmailNotifications;
using Users.Domain;

namespace Users.Api.Endpoints;

public sealed class ResendEmailConfirmation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "resend/confirmation",
            async (ISender sender, ResendRequest request) =>
        {
            ResendEmailConfirmationCommand command = new(request.UserEmail);
            ErrorOr<Unit> result = await sender.Send(command, default);

            return result.Match(
                _ => Results.Ok(),
                errors => ApiErrors.Problem(errors));
        });
    }

    private sealed record ResendRequest(string UserEmail);
}