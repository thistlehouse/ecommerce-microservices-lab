using ErrorOr;
using Inventories.Domain.StockItems;
using MediatR;

namespace Inventories.Application.Commands.PatchStockItem;

public sealed record PatchStockItemsCommand(
    List<PatchStockItemUnitsCommand> StockItems) : IRequest<ErrorOr<Unit>>;

public sealed record PatchStockItemUnitsCommand(
    Guid StockItemId,
    StockUnitOperation Operation,
    int Units);