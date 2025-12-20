using System.Net;
using Moq;
using Moq.Protected;

namespace Products.Infrastructure.UnitTests.Services.TestUtils;

public static class HttpMessageHandlerMocker
{
    public static (Mock<HttpMessageHandler>, Mock<IHttpClientFactory>) MockIHttpClientFactory(HttpStatusCode statusCode)
    {
        Mock<HttpMessageHandler> _mockHttpMessageHandler = new();

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(statusCode));

        HttpClient client = new(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        Mock<IHttpClientFactory> factory = new();

        factory.Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(client);

        return (_mockHttpMessageHandler, factory);
    }
}