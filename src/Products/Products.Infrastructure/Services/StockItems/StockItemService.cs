using System.Net.Http.Json;
using Products.Application.Common.Abstractions.Services;
using Products.Application.Common.Abstractions.Services.StockItems;
using Products.Domain.Products;
using Products.Domain.StockItems;

namespace Products.Infrastructure.Services.StockItems;

public sealed class StockItemService(IHttpClientFactory factory) : IStockItemService
{
    private readonly IHttpClientFactory _factory = factory;

    public async Task CreateStockItem(Product product)
    {
        StockItemRequest stockItemRequest = new(product.Id, product.Name, 1);
        HttpClient httpClient = _factory.CreateClient("Inventory");
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/inventory", stockItemRequest);

        response.EnsureSuccessStatusCode();
    }

    public async Task<StockItem?> GetStockItemById(Guid productId)
    {
        HttpClient httpClient = _factory.CreateClient("Inventory");
        HttpResponseMessage response = await httpClient.GetAsync($"inventory/stock-items/{productId}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<StockItem>();
    }

    public async Task<List<StockItem>> GetStockItems(IEnumerable<Guid> productIds)
    {
        HttpClient httpClient = _factory.CreateClient("Inventory");
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "/inventory/stock-items/",
            new GetStockItemsRequest(productIds));

        response.EnsureSuccessStatusCode();

        List<StockItem>? stockItems = await response.Content.ReadFromJsonAsync<List<StockItem>>();
        return stockItems ?? [];
    }

    private record StockItemRequest(Guid Id, string Name, int Units);
    private sealed record GetStockItemsRequest(IEnumerable<Guid> StockItemIds);
}