using Products.Domain.Products;
using Products.Domain.StockItems;

namespace Products.Application.Common.Abstractions.Services.StockItems;

public interface IStockItemService
{
    Task<StockItem?> GetStockItemById(Guid productId);
    Task<List<StockItem>> GetStockItems(IEnumerable<Guid> productIds);
    Task CreateStockItem(Product product);
}