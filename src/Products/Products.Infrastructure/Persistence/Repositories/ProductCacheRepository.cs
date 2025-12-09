using System.Text.Json;
using Products.Application.Common.Abstractions;
using Products.Domain.Products;
using StackExchange.Redis;

namespace Products.Infrastructure.Persistence.Repositories;

public sealed class ProductCacheRepository(
    ProductRepository productRepository,
    IConnectionMultiplexer connectionMultiplexer)
    : IProductRepository
{
    private readonly ProductRepository _productRepository = productRepository;
    private readonly IDatabase cache = connectionMultiplexer.GetDatabase();

    public void Add(Product product)
    {
        _productRepository.Add(product);
    }

    public Product? GetById(Guid productId)
    {
        string? cacheKey = CreateCacheKey(productId);

        var cacheValue = cache.StringGet(cacheKey);
        if (!string.IsNullOrEmpty(cacheValue))
        {
            Product? cachedProduct = JsonSerializer.Deserialize<Product>(cacheValue.ToString());
            return cachedProduct;
        }

        Product? product = _productRepository.GetById(productId);
        if (product is null) return null;

        string jsonfiedProduct = JsonSerializer.Serialize(product);

        cache.StringSet(cacheKey, jsonfiedProduct, TimeSpan.FromMinutes(10));

        return product;
    }

    public IEnumerable<Product> GetProducts(int page, int pageSize, out int totalItems)
    {
        IEnumerable<Product> products = _productRepository.GetProducts(
            page,
            pageSize,
            out totalItems);

        return products;
    }

    public void Remove(Product product)
    {
        _productRepository.Remove(product);

        string cacheKey = CreateCacheKey(product.Id);
        cache.KeyDelete(cacheKey);
    }

    public void Update(Product product)
    {
        _productRepository.Update(product);

        string cacheKey = CreateCacheKey(product.Id);
        cache.KeyDelete(cacheKey);
    }

    private static string CreateCacheKey(Guid productId) => $"product:{productId}";
}