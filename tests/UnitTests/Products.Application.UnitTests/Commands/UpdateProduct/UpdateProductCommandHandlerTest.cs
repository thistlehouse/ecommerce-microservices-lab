using ErrorOr;
using FluentAssertions;
using Moq;
using Products.Application.Commands.UpdateProduct;
using Products.Application.Common.Abstractions;
using Products.Domain.Products;

namespace Products.Application.UnitTests.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandlerTest
{
    private readonly UpdateProductCommandHandler _handler;
    private readonly Mock<IProductRepository> _mockProductRepository;

    public UpdateProductCommandHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _handler = new UpdateProductCommandHandler(_mockProductRepository.Object);
    }

    [Fact]
    public async Task HandleUpdateCommandHandler_WhenProductIsValid_ShouldUpdateAndReturnSuccess()
    {
        Product product = Product.Create(
            "Old_ProductTestName",
            "Old_ProductTest1Description",
            100.00m);

        UpdateProductCommand command = new(
            product.Id,
            "New_ProductTestName",
            "New_ProductTestDescription",
            200.00m);

        _mockProductRepository.Setup(m => m.GetById(product.Id))
            .Returns(product);

        ErrorOr<Product> result = await _handler.Handle(command, default);

        result.Value.Should().BeOfType<Product>();
        result.Value.Name.Should().Be(command.Name);
        result.Value.Description.Should().Be(command.Description);
        result.Value.Price.Should().Be(command.Price);

        _mockProductRepository.Verify(m => m.GetById(command.ProductId), Times.Once);
        _mockProductRepository.Verify(m => m.Update(product), Times.Once);
    }

    [Fact]
    public async Task HandleUpdateCommandHandler_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        UpdateProductCommand command = new(
            Guid.NewGuid(),
            "New_ProductTestName",
            "New_ProductTestDescription",
            200.00m);

        _mockProductRepository.Setup(m => m.GetById(It.IsAny<Guid>()))
            .Returns((Product)null!);

        ErrorOr<Product> result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("ProductNotFound");

        _mockProductRepository.Verify(m => m.GetById(command.ProductId), Times.Once);
        _mockProductRepository.Verify(m => m.Update(It.IsAny<Product>()), Times.Never);
    }
}