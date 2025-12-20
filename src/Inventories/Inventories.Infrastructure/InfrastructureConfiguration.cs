using Inventories.Application;
using Inventories.Infrastructure.Persistence;
using Inventories.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventories.Infrastructure;

public static class InfrastructureConfigurations
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddPersistence(configuration);
        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddScoped<IStockItemRepository, StockItemRepository>();

        services.AddDbContext<InventoryDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("EcommerceDatabase"),
                builder => builder.MigrationsHistoryTable("__EFMigrationsHistory", "Inventories")));

        return services;
    }
}