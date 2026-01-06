using ErrorOr;
using FluentAssertions;
using Moq;
using Products.Application.Commands.CreateProducts;
using Products.Application.Common.Abstractions;
using Products.Application.Common.Abstractions.Services.StockItems;
using Products.Contracts.Products;
using Products.Domain.Products;

namespace Products.Application.UnitTests.Commands.CreateProduct;

public class CreateProductCommandHandlerTest
{
    private readonly CreateProductCommandHandler handler;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IStockItemService> _mockStockItemService;

    public CreateProductCommandHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockStockItemService = new Mock<IStockItemService>();
        handler = new CreateProductCommandHandler(_mockProductRepository.Object, _mockStockItemService.Object);
    }

    [Fact]
    public async Task HandleCreateProductCommand_WhenProductIsValid_ShouldCreateAndReturnProductIdAsync()
    {
        CreateProductCommand command = new(
            "ProductTest1",
            "ProductTest1Description",
            100.00m);

        ErrorOr<CreateProductResult> result = await handler.Handle(command, default);

        result.Should().NotBeNull();

        _mockProductRepository.Verify(m => m.Add(It.IsAny<Product>()), Times.Once);
        _mockStockItemService.Verify(m => m.CreateStockItem(It.IsAny<Product>()), Times.Once);
    }
}
