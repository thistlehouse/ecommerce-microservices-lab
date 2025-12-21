using ErrorOr;
using FluentAssertions;
using Inventories.Application.Commands.PatchStockItem;
using Inventories.Application.UnitTests.PatchStockItemUnits.TestUtils;
using Inventories.Domain.StockItems;
using MediatR;
using Moq;

namespace Inventories.Application.UnitTests.PatchStockItemUnits;

public class PatchStockItemUnitsCommandHandlerTests
{
    private readonly Mock<IStockItemRepository> _mockStockItemRepository;
    private readonly PatchStockItemUnitsCommandHandler _handler;

    public PatchStockItemUnitsCommandHandlerTests()
    {
        _mockStockItemRepository = new();
        _handler = new(_mockStockItemRepository.Object);
    }

    [Fact]
    public async Task HandlerPatchStockItemUnits_WhenAllIdsExist_ShouldPatchIncreaseUnit_AndReturnNoContent()
    {
        PatchStockItemsCommand command = PatchStockItemUnitsCommandUtils
            .CreatePatchStockItemsCommand(StockUnitOperation.Increase);

        _mockStockItemRepository.Setup(m => m.GetByIds(It.IsAny<List<Guid>>()))
            .Returns(new List<StockItem>
            {
                StockItem.Create(
                    command.StockItems[0].StockItemId,
                    "JustAName",
                    command.StockItems[0].Units)
            });

        ErrorOr<Unit> result = await _handler.Handle(command, default);

        result.Value.Should().NotBeNull();

        _mockStockItemRepository.Verify(
            m => m.GetByIds(It.IsAny<List<Guid>>()),
            Times.Once);

        _mockStockItemRepository.Verify(m => m.PatchUnits(It.IsAny<List<StockItem>>()), Times.Once);
    }

    [Fact]
    public async Task HandlerPatchStockItemUnits_WhenAllIdsExist_ShouldPatchDecreaseUnit_AndReturnNoContent()
    {
        PatchStockItemsCommand command = PatchStockItemUnitsCommandUtils
            .CreatePatchStockItemsCommand(StockUnitOperation.Decrease);

        _mockStockItemRepository.Setup(m => m.GetByIds(It.IsAny<List<Guid>>()))
            .Returns(new List<StockItem>
            {
                StockItem.Create(
                    command.StockItems[0].StockItemId,
                    "JustAName",
                    command.StockItems[0].Units)
            });

        ErrorOr<Unit> result = await _handler.Handle(command, default);

        result.Should().NotBeNull();

        _mockStockItemRepository.Verify(
            m => m.GetByIds(It.IsAny<List<Guid>>()),
            Times.Once);

        _mockStockItemRepository.Verify(m => m.PatchUnits(It.IsAny<List<StockItem>>()), Times.Once);
    }

    [Fact]
    public async Task HandlerPatchStockItemUnits_WhenIdsDoNotExist_ShouldPatchDecreaseUnit_AndReturnNotFound()
    {
        PatchStockItemsCommand command = PatchStockItemUnitsCommandUtils
            .CreatePatchStockItemsCommand(StockUnitOperation.Decrease);

        _mockStockItemRepository.Setup(m => m.GetByIds(It.IsAny<List<Guid>>()))
            .Returns(Enumerable.Empty<StockItem>());

        ErrorOr<Unit> result = await _handler.Handle(command, default);

        result.FirstError.Code.Should().Be("StockItemsNotFound");

        _mockStockItemRepository.Verify(
            m => m.GetByIds(It.IsAny<List<Guid>>()),
            Times.Once);

        _mockStockItemRepository.Verify(m => m.PatchUnits(It.IsAny<List<StockItem>>()), Times.Never);
    }
}