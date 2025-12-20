using Inventories.Domain.StockItems;

namespace Inventories.Application;

public interface IStockItemRepository
{
    void Add(StockItem stockItem);
    StockItem? GetById(Guid stockItemId);
}
