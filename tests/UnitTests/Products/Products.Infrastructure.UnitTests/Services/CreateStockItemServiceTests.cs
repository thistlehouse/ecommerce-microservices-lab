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
        var (_mockHttpMessageHandler, factory) = HttpMessageHandlerMocker.MockIHttpClientFactory(HttpStatusCode.OK);

        CreateStockItemService service = new(factory.Object);
        Func<Task> act = () => service.SendAsync(Product.Create("ProductTest", "ProductTest", 10m));

        await act.Should().NotThrowAsync();

        _mockHttpMessageHandler.Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task Service_ShouldNotSendAsyncStockItem_And_WhenResponseIsNotSuccess()
    {
        var (_mockHttpMessageHandler, factory) = HttpMessageHandlerMocker.MockIHttpClientFactory(HttpStatusCode.InternalServerError);

        CreateStockItemService service = new(factory.Object);
        Func<Task> act = () => service.SendAsync(Product.Create("ProductTest", "ProductTest", 10m));

        await act.Should().ThrowAsync();
    }
}
