using Products.Domain.Products;

namespace Products.Application.Common.Abstractions;

public interface IProductRepository
{
    void Add(Product product);
    Product? GetProductById(Guid productId);
    IEnumerable<Product> GetProducts(int page, int pageSize, out int totalItems);
    void Update(Product product);
}