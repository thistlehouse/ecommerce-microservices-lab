using Products.Application.Abstractions.Repositories;
using Products.Domain.Products;

namespace Products.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    public void Add(Product product)
    {
        ProductContext.Products.Add(product);
    }
}