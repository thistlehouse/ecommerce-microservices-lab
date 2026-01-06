using System.Net.Http.Json;
using Products.Application.Common.Abstractions.Services;

namespace Products.Infrastructure.Services;

public class ServiceTokenProvider(IHttpClientFactory factory) : IServiceTokenProvider
{
    private readonly IHttpClientFactory _factory = factory;

    public async Task<string> GetTokenAsync()
    {
        HttpClient httpClient = _factory.CreateClient("User");
        var request = new
        {
            serviceName = "product-service",
            secret = "product-service-secret-32bytes!!", // hardcoded for now
        };

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/token", request);
        string responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Token endpoint response: {response.StatusCode}");
        Console.WriteLine($"Response body: {responseContent}");
        response.EnsureSuccessStatusCode();
        TokenResponse? authToken = await response.Content.ReadFromJsonAsync<TokenResponse>();

        return authToken!.Token;
    }

    private sealed record TokenResponse(string Token);
}