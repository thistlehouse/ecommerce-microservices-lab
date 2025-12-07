using MediatR;
using Products.Application.Queries.GetProduct;

namespace Products.Api.Endpoints;

public sealed class GetProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "products/{productId}",
            async (ISender sender, Guid productId) =>
        {
            var response = await sender.Send(new GetProductByIdQuery(productId));

            return response.Match(
                product => Results.Ok(product),
                errors => ApiErrors.Problem(errors));
        });
    }
}