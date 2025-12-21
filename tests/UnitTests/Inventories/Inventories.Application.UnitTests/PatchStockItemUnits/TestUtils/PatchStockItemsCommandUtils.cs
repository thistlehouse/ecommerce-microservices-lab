using Inventories.Application.Commands.PatchStockItem;
using Inventories.Domain.StockItems;

namespace Inventories.Application.UnitTests.PatchStockItemUnits.TestUtils;

public static class PatchStockItemUnitsCommandUtils
{
    public static PatchStockItemsCommand CreatePatchStockItemsCommand(StockUnitOperation operation = StockUnitOperation.Increase)
    {
        return new PatchStockItemsCommand(CreatePatchStockItemUnitsCommand(operation: operation));
    }

    public static List<PatchStockItemUnitsCommand> CreatePatchStockItemUnitsCommand(
        int count = 3,
        int units = 1,
        StockUnitOperation operation = StockUnitOperation.Increase) =>
            Enumerable.Range(0, count)
                .Select(index => new PatchStockItemUnitsCommand(
                    Guid.NewGuid(),
                    operation,
                    units))
                .ToList();
}