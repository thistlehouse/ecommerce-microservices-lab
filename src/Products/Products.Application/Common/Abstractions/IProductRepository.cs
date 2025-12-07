using Products.Domain.Products;

namespace Products.Application.Common.Abstractions;

public interface IProductRepository
{
    void Add(Product product);
    Product? GetProductById(Guid productId);
}