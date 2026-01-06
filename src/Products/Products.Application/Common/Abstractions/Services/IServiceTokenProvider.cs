namespace Products.Application.Common.Abstractions.Services;

public interface IServiceTokenProvider
{
    Task<string> GetTokenAsync();
}