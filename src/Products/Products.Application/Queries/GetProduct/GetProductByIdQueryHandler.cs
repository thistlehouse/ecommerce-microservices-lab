using ErrorOr;
using MediatR;
using Products.Application.Common.Abstractions;
using Products.Application.Common.Abstractions.Services.StockItems;
using Products.Application.Queries.Common;
using Products.Domain.Products;
using Products.Domain.StockItems;

namespace Products.Application.Queries.GetProduct;

public sealed class GetProductByIdQueryHandler(
    IProductRepository productRepository,
    IStockItemService stockItemService)
    : IRequestHandler<GetProductByIdQuery, ErrorOr<ProductResult>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IStockItemService _stockItemService = stockItemService;

    public async Task<ErrorOr<ProductResult>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (_productRepository.GetById(query.ProductId) is not Product product)
        {
            return Error.NotFound("ProductNotFound", "Product not found.");
        }

        StockItem? stockItem = null;

        try
        {
            stockItem = await _stockItemService.GetStockItemById(product.Id);
        }
        catch (Exception)
        {
            // TODO: Log
        }

        ProductResult productResult = new(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            stockItem?.Units);

        return productResult;
    }
}