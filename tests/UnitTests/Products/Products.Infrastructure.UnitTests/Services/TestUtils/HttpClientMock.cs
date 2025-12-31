using System.Net;
using Moq;
using Moq.Protected;

namespace Products.Infrastructure.UnitTests.Services.TestUtils;

public sealed class HttpClientMock
{
    public Mock<IHttpClientFactory> Factory { get; }
    public Mock<HttpMessageHandler> Handler { get; }

    private HttpClientMock(
        Mock<IHttpClientFactory> factory,
        Mock<HttpMessageHandler> handler)
    {
        Factory = factory;
        Handler = handler;
    }

    public static HttpClientMock Create(
        HttpStatusCode statusCode,
        string baseUrl = "http://localhost")
    {
        var handler = new Mock<HttpMessageHandler>();

        handler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(statusCode));

        var client = new HttpClient(handler.Object)
        {
            BaseAddress = new Uri(baseUrl)
        };

        var factory = new Mock<IHttpClientFactory>();

        factory
            .Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(client);

        return new HttpClientMock(factory, handler);
    }
}