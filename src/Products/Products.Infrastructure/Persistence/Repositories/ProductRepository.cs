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

    public IEnumerable<Product> GetProducts(int page, int pageSize, out int totalItems)
    {
        totalItems = ProductContext.Products.Count;

        IEnumerable<Product> products = ProductContext.Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return products;
    }

    public void Update(Product product)
    {
        int index = ProductContext.Products.IndexOf(product);
        ProductContext.Products[index] = product;
    }
}