using ErrorOr;
using FluentAssertions;
using Inventories.Application.Commands.CreateStockItem;
using Inventories.Domain.StockItems;
using MediatR;
using Moq;

namespace Inventories.Application.UnitTests.CreateStockItem;

public class CreateStockItemCommandHandlerTests
{
    private readonly Mock<IStockItemRepository> _mockStockItemRepository;
    private readonly CreateStockItemCommandHandler _handler;

    public CreateStockItemCommandHandlerTests()
    {
        _mockStockItemRepository = new();
        _handler = new CreateStockItemCommandHandler(_mockStockItemRepository.Object);
    }

    [Fact]
    public async Task HandlerCreateStockItem_WhenStockItemIsValid_ShouldCreateAndReturnUnitAsync()
    {
        CreateStockItemCommand command = new(Guid.NewGuid(), "StockItemName", 10);

        ErrorOr<Unit> result = await _handler.Handle(command, default);

        result.Should().NotBeNull();

        _mockStockItemRepository.Verify(m => m.GetById(It.IsAny<Guid>()), Times.Once);
        _mockStockItemRepository.Verify(m => m.Add(It.IsAny<StockItem>()), Times.Once);
    }

    [Fact]
    public async Task HandlerCreateStockItem_WhenStockItemExists_ShouldNotCreateAndReturnConflict()
    {
        CreateStockItemCommand command = new(Guid.NewGuid(), "StockItemName", 10);

        _mockStockItemRepository.Setup(m => m.GetById(It.IsAny<Guid>()))
            .Returns(StockItem.Create(
                command.Id,
                command.Name,
                command.Units));

        ErrorOr<Unit> result = await _handler.Handle(command, default);

        result.Should().NotBeNull();
        result.FirstError.Code.Should().Be("StockItemExists");

        _mockStockItemRepository.Verify(m => m.GetById(It.IsAny<Guid>()), Times.Once);
        _mockStockItemRepository.Verify(m => m.Add(It.IsAny<StockItem>()), Times.Never);
    }
}
