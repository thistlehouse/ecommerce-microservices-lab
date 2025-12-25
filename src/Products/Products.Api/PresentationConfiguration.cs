using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Products.Api.Permissions;

namespace Products.Api;

public static class PresentationConfiguration
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        return services
            .AddAuthentication(configuration)
            .AddAuthorization();
    }

    public static IServiceCollection AddAuthentication(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options => options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!)),
            });

        return services;
    }

    public static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

        string[] permissions = GetPermissions();

        services.AddAuthorization(config =>
        {
            foreach (string permission in permissions)
            {
                config.AddPolicy(permission, policy =>
                    policy.Requirements.Add(new PermissionRequirement(permission)));
            }
        });

        return services;
    }

    // That's kinda stupid...
    private static string[] GetPermissions()
    {
        string[] permissions =
        [
            "product.read",
            "order.create",
            "order.cancel",
            "product.create",
            "product.update",
            "product.delete",
        ];

        return permissions;
    }
}