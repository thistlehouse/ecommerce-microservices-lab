using System.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Users.Application.Common.Abstractions.Authorization;
using Users.Application.Common.Abstractions.Repositories;
using Users.Application.Common.Abstractions.Services;
using Users.Application.Common.Abstractions.Services.ConfirmationCodes;
using Users.Application.Common.Abstractions.Services.EmailNotifications;
using Users.Infrastructure.Authentication;
using Users.Infrastructure.Authorization;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Persistence.Repositories;
using Users.Infrastructure.Security;
using Users.Infrastructure.Services;
using Users.Infrastructure.Services.Confirmation;
using Users.Infrastructure.Services.EmailNotifications;

namespace Users.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IPermissionProvider, PermissionProvider>();
        services.AddTransient<IEmailNotification, EmailNotification>();
        services.AddTransient<IConfirmationCodeGenerator, ConfirmationCodeGenerator>();

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
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IServiceIdentityProvider, ServiceIdentityProvider>();

        services.Configure<ServicesSettings>(
            configuration.GetSection(ServicesSettings.SectionName));

        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICodeRepository, CodeRepository>();

        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("ECommerceDatabase"),
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure();
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "Users");
                }));

        return services;
    }
}