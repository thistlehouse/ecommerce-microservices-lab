using ErrorOr;
using MediatR;
using Products.Contracts.Products;

namespace Products.Application.Commands.CreateProducts;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price) : IRequest<ErrorOr<CreateProductResult>>;