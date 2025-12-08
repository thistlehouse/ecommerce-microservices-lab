using ErrorOr;
using MediatR;
using Products.Domain.Products;

namespace Products.Application.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price) : IRequest<ErrorOr<Product>>;