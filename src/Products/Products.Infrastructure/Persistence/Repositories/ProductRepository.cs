using Products.Application.Common.Abstractions;
using Products.Domain.Products;

namespace Products.Infrastructure.Persistence.Repositories;

public class ProductRepository(ProductDbContext context) : IProductRepository
{
    private readonly ProductDbContext _context = context;

    public void Add(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public Product? GetById(Guid productId)
    {
        return _context.Products.Find(productId);
    }

    public IEnumerable<Product> GetProducts(int page, int pageSize, out int totalItems)
    {
        totalItems = _context.Products.Count();

        IEnumerable<Product> products = _context.Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return products;
    }

    public void Remove(Product product)
    {
        _context.Products.Remove(product);
        _context.SaveChanges();
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }
}