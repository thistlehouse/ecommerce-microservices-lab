namespace Products.Application.Queries.Common;

public sealed record ProductResult(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int? Stock);