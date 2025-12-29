using Microsoft.AspNetCore.Mvc;
using Users.Application.Common.Abstractions.Repositories;
using Users.Domain;

namespace Users.Api.Endpoints;

public sealed class EmailConfirmation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "email/confirmation",
            ([FromQuery] string email, IUserRepository userRepository) =>
        {
            User? user = userRepository.GetByEmail(email);
            if (user is null)
            {
                Results.NotFound();
            }

            userRepository.ConfirmEmail(email);
        });
    }
}