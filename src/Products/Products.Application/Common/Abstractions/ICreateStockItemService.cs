using Products.Domain.Products;

namespace Products.Application.Common.Abstractions;

public interface ICreateStockItemService
{
    Task SendAsync(Product product);
}