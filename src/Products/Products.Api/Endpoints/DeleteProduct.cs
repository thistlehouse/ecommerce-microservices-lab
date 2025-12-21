using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Errors;
using Products.Application.Commands.DeleteProduct;

namespace Products.Api.Endpoints;

public sealed class DeleteProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/products/{productId}",
            async (ISender sender, [FromRoute] Guid productId) =>
        {
            DeleteProductCommand command = new(productId);

            ErrorOr<DeleteProductResult> response = await sender.Send(command, default);

            return response.Match(
                _ => Results.NoContent(),
                errors => ApiErrors.Problem(errors));
        });
    }
}