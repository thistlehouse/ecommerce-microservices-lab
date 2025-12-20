using Inventories.Domain.StockItems;
using Microsoft.EntityFrameworkCore;

namespace Inventories.Infrastructure.Persistence;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options)
    : DbContext(options)
{
    public DbSet<StockItem> StockItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("Inventories");
        builder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
        base.OnModelCreating(builder);
    }
}
