using Inventories.Domain.StockItems;

namespace Inventories.Application;

public interface IStockItemRepository
{
    void Add(StockItem stockItem);
    StockItem? GetById(Guid stockItemId);
    IEnumerable<StockItem?> GetByIds(List<Guid> guids);
    void PatchUnits(List<StockItem> stockItems);
}
