using ErrorOr;
using FluentAssertions;
using Moq;
using Products.Application.Common.Abstractions;
using Products.Application.Queries.GetProduct;
using Products.Domain.Products;

namespace Products.Application.UnitTests.Queries.GetProductById;

public sealed class GetProductByIdQueryHandlerTest
{
    private readonly GetProductByIdQueryHandler _handler;
    private readonly Mock<IProductRepository> _mockProductRepository;

    public GetProductByIdQueryHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _handler = new GetProductByIdQueryHandler(_mockProductRepository.Object);
    }

    [Fact]
    public async Task HandleGetProductByIdQuery_WhenProductExists_ShouldReturnProduct()
    {
        Product product = Product.Create(
            "ProductTest1",
            "ProductTest1Description",
            100.00m);

        _mockProductRepository.Setup(m => m.GetProductById(product.Id))
            .Returns(product);

        ErrorOr<Product> result = await _handler.Handle(
            new GetProductByIdQuery(product.Id),
            default);

        result.Value.Should().NotBeNull();

        _mockProductRepository.Verify(m => m.GetProductById(product.Id), Times.Once);
    }

    [Fact]
    public async Task HandleGetProductByIdQuery_WhenProductDoesNotExists_ShouldReturnNotFound()
    {
        _mockProductRepository.Setup(m => m.GetProductById(It.IsAny<Guid>()))
            .Returns((Product)null!);

        ErrorOr<Product> result = await _handler.Handle(
            new GetProductByIdQuery(Guid.NewGuid()),
            default);

        result.Value.Should().BeNull();
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("ProductNotFound");

        _mockProductRepository.Verify(m => m.GetProductById(It.IsAny<Guid>()), Times.Once);
    }
}