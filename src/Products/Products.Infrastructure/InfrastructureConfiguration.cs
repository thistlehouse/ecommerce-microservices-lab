using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Application.Common.Abstractions;
using Products.Application.Common.Abstractions.Services;
using Products.Application.Common.Abstractions.Services.StockItems;
using Products.Infrastructure.Persistence;
using Products.Infrastructure.Persistence.Repositories;
using Products.Infrastructure.Services;
using Products.Infrastructure.Services.StockItems;
using StackExchange.Redis;

namespace Products.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddPersistence(configuration);
        services.AddCache(configuration);
        services.AddServices();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IStockItemService, StockItemService>();
        services.AddTransient<ServiceAuthenticationHandler>();
        services.AddScoped<IServiceTokenProvider, ServiceTokenProvider>();

        return services;
    }

    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddDbContext<ProductDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("ECommerceDatabase"),
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure();
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "Products");
                }));

        return services;
    }

    public static IServiceCollection AddCache(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(
            configuration.GetConnectionString("Redis")!);

        services.Configure<CacheSettings>(configuration.GetSection("ExpirationMinutes"));
        services.AddSingleton(connectionMultiplexer);
        services.Decorate<IProductRepository, ProductCacheRepository>();

        return services;
    }
}