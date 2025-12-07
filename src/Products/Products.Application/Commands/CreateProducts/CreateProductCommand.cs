using MediatR;

namespace Products.Application.Commands.CreateProducts;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price) : IRequest<CreateProductResponse>;