using Bogus;
using Products.Domain.Products;

namespace Products.Infrastructure.Persistence.Seeds;

public sealed class ProductFaker
{
    public static Faker<Product> Faker = new Faker<Product>()
        .CustomInstantiator(f => Product.Create(
            f.Commerce.ProductName(),
            f.Lorem.Sentences(),
            Math.Round(f.Random.Decimal(100, 9999), 2)));
}