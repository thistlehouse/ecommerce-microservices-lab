using Products.Domain.Products;

namespace Products.Application.Common.Abstractions;

public interface IProductRepository
{
    void Add(Product product);
    Product? GetById(Guid productId);
    IEnumerable<Product> GetProducts(int page, int pageSize, out int totalItems);
    void Update(Product product);
    void Remove(Product product);
}