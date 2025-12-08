using ErrorOr;
using FluentAssertions;
using Moq;
using Products.Application.Commands.DeleteProduct;
using Products.Application.Common.Abstractions;
using Products.Domain.Products;

namespace Products.Application.UnitTests.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandlerTests
{
    private readonly DeleteProductCommandHandler _handler;
    private readonly Mock<IProductRepository> _mockProductRepository;

    public DeleteProductCommandHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _handler = new DeleteProductCommandHandler(_mockProductRepository.Object);
    }

    [Fact]
    public async Task HandleDeleteProductCommand_WhenProductExists_ShouldDeleteAndReturnNoContent()
    {
        Product product = Product.Create(
            "Old_ProductTestName",
            "Old_ProductTest1Description",
            100.00m);

        DeleteProductCommand command = new(product.Id);

        _mockProductRepository.Setup(m => m.GetById(It.IsAny<Guid>()))
            .Returns(product);

        ErrorOr<DeleteProductResult> result = await _handler.Handle(command, default);

        result.Value.Should().NotBeNull();

        _mockProductRepository.Verify(m => m.GetById(product.Id), Times.Once);
        _mockProductRepository.Verify(m => m.Remove(product), Times.Once);
    }

    [Fact]
    public async Task HandleDeleteProductCommand_WhenProductDoesNotExist_ShouldDeleteAndReturnNotFound()
    {
        DeleteProductCommand command = new(Guid.NewGuid());

        _mockProductRepository.Setup(m => m.GetById(It.IsAny<Guid>()))
            .Returns((Product)null!);

        ErrorOr<DeleteProductResult> result = await _handler.Handle(command, default);

        result.Value.Should().BeNull();
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("ProductNotFound");

        _mockProductRepository.Verify(m => m.GetById(It.IsAny<Guid>()), Times.Once);
        _mockProductRepository.Verify(m => m.Remove(It.IsAny<Product>()), Times.Never);
    }
}