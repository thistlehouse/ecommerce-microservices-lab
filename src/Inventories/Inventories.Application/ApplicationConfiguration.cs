using Microsoft.Extensions.DependencyInjection;

namespace Inventories.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(typeof(ApplicationConfiguration).Assembly));

        return services;
    }
}