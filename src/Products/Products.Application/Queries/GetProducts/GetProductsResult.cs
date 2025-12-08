using Products.Domain.Products;

namespace Products.Application.Queries.GetProducts;

public record GetProductsResult(
    List<Product> Products,
    int PageIndex,
    int PageSize,
    int TotalItems)
{
    public int TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
    public bool HasPreviousPage = PageIndex > 1;
    public bool HasNextPage = PageIndex < TotalItems;
};