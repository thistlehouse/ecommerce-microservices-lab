using ErrorOr;
using MediatR;
using Products.Domain.Products;

namespace Products.Application.Queries.GetProduct;

public record GetProductByIdQuery(Guid ProductId) : IRequest<ErrorOr<Product>>;