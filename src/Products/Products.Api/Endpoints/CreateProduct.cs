using MediatR;
using Products.Application.Commands.CreateProducts;

namespace Products.Api.Endpoints;

public sealed class CreateProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "products/create",
            async (ISender sender, CreateProductRequest request) =>
        {
            CreateProductCommand command = new(request.Name, request.Description, request.Price);

            var response = await sender.Send(command);

            return response.Match(
                product => Results.Created("products/create", product),
                errors => ApiErrors.Problem(errors));
        });
    }

    public record CreateProductRequest(string Name, string Description, decimal Price);
}