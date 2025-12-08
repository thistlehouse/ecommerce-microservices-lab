
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Errors;
using Products.Application.Queries.GetProducts;

namespace Products.Api.Endpoints;

public sealed class GetProducts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/products",
            async (ISender sender, [FromQuery] int page, [FromQuery] int pageSize) =>
        {
            GetProductsQuery query = new(page, pageSize);

            ErrorOr<GetProductsResult> response = await sender.Send(query);

            return response.Match(
                products => Results.Ok(products),
                errors => ApiErrors.Problem(errors));
        });
    }
}