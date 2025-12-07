using Products.Domain.Products;

namespace Products.Application.Abstractions.Repositories;

public interface IProductRepository
{
    void Add(Product product);
}