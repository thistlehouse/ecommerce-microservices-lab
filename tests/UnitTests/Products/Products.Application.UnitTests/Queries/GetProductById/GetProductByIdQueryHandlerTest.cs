using ErrorOr;
using FluentAssertions;
using Moq;
using Products.Application.Common.Abstractions;
using Products.Application.Common.Abstractions.Services.StockItems;
using Products.Application.Queries.Common;
using Products.Application.Queries.GetProduct;
using Products.Domain.Products;

namespace Products.Application.UnitTests.Queries.GetProductById;

public sealed class GetProductByIdQueryHandlerTest
{
    private readonly GetProductByIdQueryHandler _handler;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IStockItemService> _mockStockItemService;

    public GetProductByIdQueryHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockStockItemService = new Mock<IStockItemService>();
        _handler = new GetProductByIdQueryHandler(_mockProductRepository.Object, _mockStockItemService.Object);
    }

    [Fact]
    public async Task HandleGetProductByIdQuery_WhenProductExists_ShouldReturnProduct()
    {
        Product product = Product.Create(
            "ProductTest1",
            "ProductTest1Description",
            100.00m);

        _mockProductRepository.Setup(m => m.GetById(product.Id))
            .Returns(product);

        ErrorOr<ProductResult> result = await _handler.Handle(
            new GetProductByIdQuery(product.Id),
            default);

        result.Value.Should().NotBeNull();

        _mockProductRepository.Verify(m => m.GetById(product.Id), Times.Once);
    }

    [Fact]
    public async Task HandleGetProductByIdQuery_WhenProductDoesNotExists_ShouldReturnNotFound()
    {
        _mockProductRepository.Setup(m => m.GetById(It.IsAny<Guid>()))
            .Returns((Product)null!);

        ErrorOr<ProductResult> result = await _handler.Handle(
            new GetProductByIdQuery(Guid.NewGuid()),
            default);

        result.Value.Should().BeNull();
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("ProductNotFound");

        _mockProductRepository.Verify(m => m.GetById(It.IsAny<Guid>()), Times.Once);
    }
}