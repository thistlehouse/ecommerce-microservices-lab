using ErrorOr;
using FluentAssertions;
using Moq;
using Products.Application.Common.Abstractions;
using Products.Application.Common.Abstractions.Services.StockItems;
using Products.Application.Queries.GetProducts;
using Products.Domain.Products;
using Products.Domain.StockItems;

namespace Products.Application.UnitTests.Queries.GetProducts;

public sealed class GetProductsQueryHandlerTest
{
    private readonly GetProductsQueryHandler _handler;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IStockItemService> _mockStockItemService;

    public GetProductsQueryHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockStockItemService = new Mock<IStockItemService>();
        _handler = new GetProductsQueryHandler(_mockProductRepository.Object, _mockStockItemService.Object);
    }

    [Fact]
    public async Task HandleGetProductsQuery_WhenProductsExist_ShouldReturnProducts()
    {
        IEnumerable<Product> products = Enumerable.Range(0, 100)
            .Select(idx => Product.Create(
                $"ProductName_{idx}",
                $"ProductDescription_{idx}",
                new Random().Next(5, 1000)));

        GetProductsQuery query = new(1, 10);

        _mockProductRepository.Setup(m => m.GetProducts(query.Page, query.PageSize, out It.Ref<int>.IsAny))
            .Callback((int page, int pageSize, out int totalItems) =>
            {
                totalItems = products.Count();
            })
            .Returns(products);

        _mockStockItemService.Setup(m => m.GetStockItems(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(new List<StockItem>());

        ErrorOr<GetProductsResult> result = await _handler.Handle(
            query,
            default);

        result.IsError.Should().BeFalse();
        result.Value.Products.Should().NotBeNull();
        result.Value.HasPreviousPage.Should().BeFalse();
        result.Value.HasNextPage.Should().BeTrue();
        result.Value.TotalItems.Should().Be(result.Value.Products.Count);
        result.Value.TotalPages.Should().Be((int)Math.Ceiling((double)result.Value.TotalItems / result.Value.PageSize));
    }

    [Fact]
    public async Task HandleGetProductsQuery_WhenProductsDoesNotExist_ShouldReturnNotFound()
    {
        int totalItems = 0;
        GetProductsQuery query = new(1, 10);

        _mockProductRepository.Setup(m => m.GetProducts(query.Page, query.PageSize, out totalItems))
            .Returns([]);

        ErrorOr<GetProductsResult> result = await _handler.Handle(
            query,
            default);

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("ProductsNotFound");
    }
}