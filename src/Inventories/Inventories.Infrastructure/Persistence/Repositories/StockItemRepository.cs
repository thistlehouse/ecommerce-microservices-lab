using Inventories.Application;
using Inventories.Domain.StockItems;
using Microsoft.EntityFrameworkCore;

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

    public IEnumerable<StockItem?> GetByIds(IEnumerable<Guid> guids)
    {
        IEnumerable<StockItem?> items = _context.StockItems.Where(s => guids.Contains(s.Id));
        return items;
    }

    public void PatchUnits(IEnumerable<StockItem> stockItems)
    {
        _context.StockItems.ExecuteUpdate(s => s.SetProperty(si => si.Units, si => si.Units));
        _context.SaveChanges();
    }
}