using ErrorOr;
using Inventories.Application.Commands.CreateStockItem;
using Inventories.Domain.StockItems;
using MediatR;

namespace Inventories.Application.Commands.CreateStockItem;

public sealed class CreateStockItemCommandHandler(IStockItemRepository stockItemRepository)
    : IRequestHandler<CreateStockItemCommand, ErrorOr<Unit>>
{
    private readonly IStockItemRepository _stockItemRepository = stockItemRepository;

    public async Task<ErrorOr<Unit>> Handle(CreateStockItemCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        StockItem? item = _stockItemRepository.GetById(command.Id);
        if (item is not null)
        {
            return Error.Conflict("StockItemExists", "Stock Item already exists");
        }

        StockItem stockItem = StockItem.Create(command.Id, command.Name, command.Units);

        _stockItemRepository.Add(stockItem);
        return new Unit();
    }
}