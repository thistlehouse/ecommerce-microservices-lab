using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain.Products;

namespace Products.Infrastructure.Persistence.Configurations;

public sealed class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasIndex(p => p.Id);
    }
}