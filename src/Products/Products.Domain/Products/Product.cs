using System.Text.Json.Serialization;

namespace Products.Domain.Products;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    // Todo: Add CreatedAt and UpdatedAt

    private Product() { }

    [JsonConstructor]
    private Product(Guid id, string name, string description, decimal price)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
    }

    public void Update(string name, string description, decimal price)
    {
        Name = name;
        Description = description;
        Price = price;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetDescription(string description)
    {
        Description = description;
    }

    public void SetPrice(decimal price)
    {
        Price = price;
    }

    public static Product Create(string name, string description, decimal price)
    {
        return new(Guid.NewGuid(), name, description, price);
    }

}