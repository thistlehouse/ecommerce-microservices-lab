using Products.Application.Common.Abstractions;
using Products.Domain.Products;

namespace Products.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    public void Add(Product product)
    {
        ProductContext.Products.Add(product);
    }

    public Product? GetProductById(Guid productId)
    {
        return ProductContext.Products.Find(p => p.Id == productId);
    }
}