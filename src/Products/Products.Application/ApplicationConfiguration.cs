using Microsoft.Extensions.DependencyInjection;

namespace Products.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(ApplicationConfiguration).Assembly);
        });

        return services;
    }
}