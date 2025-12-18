using Products.Domain.Products;

namespace Products.Application.Common.Abstractions;

public interface ICreateItemStockService
{
    Task SendAsync(Product product);
}