using System.Net;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Products.Domain.Products;
using Products.Infrastructure.Services;

namespace Products.Infrastructure.UnitTests;

public class CreateItemStockServiceTests
{

    [Fact]
    public async Task Service_ShouldSendAsyncItemStock_And_WhenResponseIs200()
    {
        Mock<HttpMessageHandler> _mockHttpMessageHandler = new();

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        HttpClient client = new(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        Mock<IHttpClientFactory> factory = new();

        factory.Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(client);

        CreateItemStockService service = new(factory.Object);
        Func<Task> act = () => service.SendAsync(Product.Create("ProductTest", "ProductTest", 10m));

        await act.Should().NotThrowAsync();

        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task Service_ShouldNotSendAsyncItemStock_And_WhenResponseIsNotSuccess()
    {
        Mock<HttpMessageHandler> _mockHttpMessageHandler = new();

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        HttpClient client = new(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        Mock<IHttpClientFactory> factory = new();

        factory.Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(client);

        CreateItemStockService service = new(factory.Object);
        Func<Task> act = () => service.SendAsync(Product.Create("ProductTest", "ProductTest", 10m));

        await act.Should().ThrowAsync();
    }
}
