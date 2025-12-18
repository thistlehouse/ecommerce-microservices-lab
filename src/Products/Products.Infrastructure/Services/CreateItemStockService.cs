using System.Net.Http.Json;
using Products.Application.Common.Abstractions;
using Products.Domain.Products;

namespace Products.Infrastructure.Services;

public sealed class CreateItemStockService(IHttpClientFactory factory) : ICreateItemStockService
{
    private readonly IHttpClientFactory _factory = factory;

    public async Task SendAsync(Product product)
    {
        ItemStockRequest itemStockRequest = new(product.Id, product.Name, 1);
        HttpClient httpClient = _factory.CreateClient("Inventory");
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/inventory", itemStockRequest);

        response.EnsureSuccessStatusCode();
    }

    private record ItemStockRequest(Guid Id, string Name, int Units);
}
