using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Users.Application.Common.Abstractions.Repositories;
using Users.Application.Common.Abstractions.Services;
using Users.Application.Common.Abstractions.Services.EmailNotifications;
using Users.Infrastructure.Authentication;
using Users.Infrastructure.Authorization;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Persistence.Repositories;
using Users.Infrastructure.Services;
using Users.Infrastructure.Services.EmailNotifications;

namespace Users.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailNotification, EmailNotification>();

        services
            .AddPersistence(configuration)
            .AddAuthentication(configuration);

        return services;
    }

    private static IServiceCollection AddAuthentication(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        JwtSettings jwtSettings = new();
        PermissionSettings permissionSettings = new();

        configuration.Bind(JwtSettings.SectionName, jwtSettings);
        configuration.Bind(PermissionSettings.SectionName, permissionSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton(Options.Create(permissionSettings));

        services.Configure<ServiceIdentitiesSettings>(
            configuration.GetSection(ServiceIdentitiesSettings.SectionName));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IServiceIdentityProvider, ServiceIdentityProvider>();

        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("ECommerceDatabase"),
                b => b.MigrationsHistoryTable("__EFMigrationsHistory", "Users")));

        return services;
    }
}