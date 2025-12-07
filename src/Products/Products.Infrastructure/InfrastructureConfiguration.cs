using Microsoft.Extensions.DependencyInjection;
using Products.Application.Abstractions.Repositories;
using Products.Infrastructure.Persistence.Repositories;

namespace Products.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}