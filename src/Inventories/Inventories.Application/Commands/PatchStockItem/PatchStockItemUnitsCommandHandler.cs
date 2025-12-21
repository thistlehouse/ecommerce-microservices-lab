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
        // TODO
        // Treat scenario where ids might be a miss

        await Task.CompletedTask;

        List<Guid> stockItemIds = [.. command.StockItems.Select(s => s.StockItemId)];
        List<StockItem?> stockItems = _stockItemRepository.GetByIds(stockItemIds).ToList();
        if (stockItems.Count == 0)
        {
            return Error.NotFound("StockItemsNotFound", "Stock items was not found");
        }

        var dictCommandItems = command.StockItems.ToDictionary(c => c.StockItemId);

        foreach (StockItem? stockItem in stockItems)
        {
            var commandItem = dictCommandItems[stockItem!.Id];
            stockItem.SetUnits(commandItem.Operation, commandItem.Units);
        }

        _stockItemRepository.PatchUnits(stockItems!);

        return new Unit();
    }
}