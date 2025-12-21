using ErrorOr;
using Inventories.Domain.StockItems;
using MediatR;

namespace Inventories.Application.Commands.PatchStockItem;

public sealed class PatchStockItemUnitsCommandHandler(IStockItemRepository stockItemRepository)
    : IRequestHandler<PatchStockItemsCommand, ErrorOr<Unit>>
{
    private readonly IStockItemRepository _stockItemRepository = stockItemRepository;

    public async Task<ErrorOr<Unit>> Handle(PatchStockItemsCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        List<Guid> stockItemIds = [.. command.StockItems.Select(s => s.StockItemId)];
        List<StockItem?> stockItems = _stockItemRepository.GetByIds(stockItemIds).ToList();

        // TODO
        // All ids must return a stock item
        // If one is a miss, an error must be returned.
        if (stockItems.Count == 0)
        {
            return Error.NotFound("StockItemsNotFound", "Stock items was not found");
        }

        foreach (StockItem? stockItem in stockItems)
        {
            (StockUnitOperation operation, int units) = command.StockItems
                .Where(c => c.StockItemId == stockItem.Id)
                .Select(c => (c.Operation, c.Units))
                .First();

            stockItem.SetUnits(operation, units);
        }

        _stockItemRepository.PatchUnits(stockItems);

        return new Unit();
    }
}