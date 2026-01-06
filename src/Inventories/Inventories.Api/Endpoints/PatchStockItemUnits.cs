using ErrorOr;
using Inventories.Api.Errors;
using Inventories.Api.Permissions;
using Inventories.Application.Commands.PatchStockItem;
using Inventories.Domain.StockItems;
using MediatR;

namespace Inventories.Api.Endpoints;

public sealed class PatchStockItemUnits : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(
            "/inventory",
            async (ISender sender, PatchStockItemsRequest request) =>
        {
            PatchStockItemsCommand command = new(
                request.StockItems.ConvertAll(s => new PatchStockItemUnitsCommand(
                    s.StockItemId,
                    s.Operation,
                    s.Units)));

            ErrorOr<Unit> result = await sender.Send(command, default);

            return result.Match(
                _ => Results.NoContent(),
                errors => ApiErrors.Problem(errors));
        })
        .RequireAuthorization(Permission.InventoryWrite);
    }

    private sealed record PatchStockItemsRequest(List<PatchStockItemUnitsRequest> StockItems);

    private sealed record PatchStockItemUnitsRequest(
        Guid StockItemId,
        StockUnitOperation Operation,
        int Units);

}