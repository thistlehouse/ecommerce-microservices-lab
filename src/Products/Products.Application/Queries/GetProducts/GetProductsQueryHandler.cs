using ErrorOr;
using MediatR;
using Products.Application.Common.Abstractions;
using Products.Domain.Products;

namespace Products.Application.Queries.GetProducts;

public sealed class GetProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductsQuery, ErrorOr<GetProductsResult>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<ErrorOr<GetProductsResult>> Handle(
        GetProductsQuery query,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        IEnumerable<Product> dbProducts = _productRepository.GetProducts(
            query.Page,
            query.PageSize,
            out int totalItems);

        List<Product> products = dbProducts.ToList();

        if (products.Count == 0)
        {
            return Error.NotFound("ProductsNotFound", "There are no Products registered");
        }

        return new GetProductsResult(products, query.Page, query.PageSize, totalItems);
    }
}