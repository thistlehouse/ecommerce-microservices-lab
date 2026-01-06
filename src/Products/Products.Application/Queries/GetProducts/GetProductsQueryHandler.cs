using ErrorOr;
using MediatR;
using Products.Application.Common.Abstractions;
using Products.Application.Common.Abstractions.Services.StockItems;
using Products.Application.Queries.Common;
using Products.Domain.Products;
using Products.Domain.StockItems;

namespace Products.Application.Queries.GetProducts;

public sealed class GetProductsQueryHandler(
    IProductRepository productRepository,
    IStockItemService stockItemService)
    : IRequestHandler<GetProductsQuery, ErrorOr<GetProductsResult>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IStockItemService _stockItemService = stockItemService;

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

        List<ProductResult> productResults = new();
        List<Guid> productIds = products.Select(p => p.Id).ToList();

        try
        {
            List<StockItem> stockItems = await _stockItemService.GetStockItems(productIds);
            bool hasCompleteInventory = stockItems.Count == products.Count;
            if (hasCompleteInventory)
            {
                Dictionary<Guid, int> stockByProductId = stockItems.ToDictionary(s => s.Id, s => s.Units);
                productResults = MapWithStock(products, stockByProductId);

                return new GetProductsResult(
                    productResults,
                    query.Page,
                    query.PageSize,
                    totalItems);
            }
        }
        catch (HttpRequestException)
        {
            // TODO: Log exception
        }

        productResults = MapWithoutStock(products);
        return new GetProductsResult(productResults, query.Page, query.PageSize, totalItems);
    }

    private List<ProductResult> MapWithoutStock(IEnumerable<Product> products) =>
        products.Select(p => new ProductResult(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            default))
        .ToList();

    private List<ProductResult> MapWithStock(
        IEnumerable<Product> products,
        Dictionary<Guid, int> stockByProductId) =>
        products.Select(p => new ProductResult(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            stockByProductId[p.Id]))
        .ToList();
}