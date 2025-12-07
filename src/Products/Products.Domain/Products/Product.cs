namespace Products.Domain.Products;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    private Product(string name, string description, decimal price, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
    }

    public static Product Create(string name, string description, decimal price)
    {
        return new(name, description, price);
    }
}