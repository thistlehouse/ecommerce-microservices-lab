using ErrorOr;
using MediatR;
using Products.Domain.Products;

namespace Products.Application.Queries.GetProducts;

public record GetProductsQuery(int Page, int PageSize) : IRequest<ErrorOr<GetProductsResult>>;