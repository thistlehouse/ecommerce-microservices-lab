using System.Net;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Products.Domain.Products;
using Products.Infrastructure.Services;
using Products.Infrastructure.UnitTests.Services.TestUtils;

namespace Products.Infrastructure.UnitTests.Services;

public class CreateStockItemServiceTests
{

    [Fact]
    public async Task Service_ShouldSendAsyncStockItem_And_WhenResponseIs200()
    {
        var httpMock = HttpClientMock.Create(HttpStatusCode.OK);
        CreateStockItemService service = new(httpMock.Factory.Object);

        Func<Task> act = () => service.SendAsync(Product.Create("ProductTest", "ProductTest", 10m));

        await act.Should().NotThrowAsync();

        httpMock.Handler.Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task Service_ShouldNotSendAsyncStockItem_And_WhenResponseIsNotSuccess()
    {
        var httpMock = HttpClientMock.Create(HttpStatusCode.OK);
        CreateStockItemService service = new(httpMock.Factory.Object);

        Func<Task> act = () => service.SendAsync(Product.Create("ProductTest", "ProductTest", 10m));

        await act.Should().ThrowAsync();
    }
}
