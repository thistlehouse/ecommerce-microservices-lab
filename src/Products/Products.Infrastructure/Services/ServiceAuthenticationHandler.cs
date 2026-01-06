using System.Net.Http.Headers;
using Products.Application.Common.Abstractions.Services;

namespace Products.Infrastructure.Services;

public sealed class ServiceAuthenticationHandler(IServiceTokenProvider serviceTokenProvider)
    : DelegatingHandler
{
    private readonly IServiceTokenProvider _serviceTokenProvider = serviceTokenProvider;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string token = await _serviceTokenProvider.GetTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}