using Products.Domain.Products;

namespace Products.Infrastructure.Persistence.Seeds;

public static class DatabaseInitialiser
{
    public static void Seed(ProductDbContext context)
    {
        if (!context.Products.Any())
        {
            List<Product> products = ProductFaker.Faker.Generate(100);
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}