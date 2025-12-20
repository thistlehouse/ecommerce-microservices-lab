using System.Net.Http.Json;
using Products.Application.Common.Abstractions;
using Products.Domain.Products;

namespace Products.Infrastructure.Services;

public sealed class CreateStockItemService(IHttpClientFactory factory) : ICreateStockItemService
{
    private readonly IHttpClientFactory _factory = factory;

    public async Task SendAsync(Product product)
    {
        StockItemRequest stockItemRequest = new(product.Id, product.Name, 1);
        HttpClient httpClient = _factory.CreateClient("Inventory");
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/inventory", stockItemRequest);

        response.EnsureSuccessStatusCode();
    }

    private record StockItemRequest(Guid Id, string Name, int Units);
}
