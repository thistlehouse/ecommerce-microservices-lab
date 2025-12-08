
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Errors;
using Products.Application.Commands.UpdateProduct;
using Products.Domain.Products;

namespace Products.Api.Endpoints;

public sealed class UpdateProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/products/{productId}",
            async (ISender sender, [FromRoute] Guid productId, UpdateRequest request) =>
        {
            UpdateProductCommand command = new(
                productId,
                request.Name,
                request.Description,
                request.Price);

            ErrorOr<Product> response = await sender.Send(command, default);

            return response.Match(
                product => Results.NoContent(),
                errors => ApiErrors.Problem(errors));
        });
    }
}

public sealed record UpdateRequest(
    int Guid,
    string Name,
    string Description,
    decimal Price);