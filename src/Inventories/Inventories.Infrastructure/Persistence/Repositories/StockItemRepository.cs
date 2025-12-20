using Inventories.Application;
using Inventories.Domain.StockItems;

namespace Inventories.Infrastructure.Persistence.Repositories;

public sealed class StockItemRepository(InventoryDbContext context) : IStockItemRepository
{
    private readonly InventoryDbContext _context = context;

    public void Add(StockItem stockItem)
    {
        _context.StockItems.Add(stockItem);
        _context.SaveChanges();
    }

    public StockItem? GetById(Guid stockItemId)
    {
        StockItem? stockItem = _context.StockItems.Find(stockItemId);
        return stockItem;
    }
}