using ErrorOr;
using MediatR;
using Products.Application.Common.Abstractions;
using Products.Domain.Products;

namespace Products.Application.Queries.GetProduct;

public sealed class GetProductByIdQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductByIdQuery, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<ErrorOr<Product>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (_productRepository.GetById(query.ProductId) is not Product product)
        {
            return Error.NotFound("ProductNotFound", "Product not found.");
        }

        return product;
    }
}