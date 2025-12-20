using ErrorOr;
using MediatR;

namespace Inventories.Application.Commands.CreateStockItem;

public record CreateStockItemCommand(
    Guid Id,
    string Name,
    int Units) : IRequest<ErrorOr<Unit>>;