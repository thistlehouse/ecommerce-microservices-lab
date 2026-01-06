using Inventories.Api.Permissions;
using Inventories.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Inventories.Api.Endpoints;

public sealed class GetStockItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inventory/stock-items/{id}", (InventoryDbContext context, [FromRoute] Guid id) =>
        {
            return Results.Ok(context.StockItems.Find(id));
        })
        .RequireAuthorization(Permission.InventoryRead);
    }
}