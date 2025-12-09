using Microsoft.EntityFrameworkCore;
using Products.Domain.Products;

namespace Products.Infrastructure.Persistence;

public class ProductDbContext(DbContextOptions<ProductDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("Products");
        builder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);

        base.OnModelCreating(builder);
    }
}