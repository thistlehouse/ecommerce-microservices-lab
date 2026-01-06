using ErrorOr;
using MediatR;
using Products.Application.Queries.Common;
using Products.Domain.Products;

namespace Products.Application.Queries.GetProduct;

public record GetProductByIdQuery(Guid ProductId) : IRequest<ErrorOr<ProductResult>>;