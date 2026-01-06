using Inventories.Api.Permissions;
using Inventories.Domain.StockItems;
using Inventories.Infrastructure.Persistence;

namespace Inventories.Api.Endpoints;

public sealed class GetStockItems : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("inventory/stock-items/", (InventoryDbContext context, GetStockItemsRequest request) =>
        {
            List<StockItem> items = context
                .StockItems
                .Where(i => request.StockItemIds.Contains(i.Id))
                .ToList();

            if (items.Count > 0)
            {
                return Results.Ok(items);
            }

            return Results.NotFound();
        })
        .RequireAuthorization(Permission.InventoryRead);
    }

    private sealed record GetStockItemsRequest(List<Guid> StockItemIds);
}