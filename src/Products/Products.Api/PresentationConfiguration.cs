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
            .AddAuthN(configuration)
            .AddAuthZ();
    }

    public static IServiceCollection AddAuthN(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new()
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

    public static IServiceCollection AddAuthZ(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

        string[] permissions = GetPermissions();

        services.AddAuthorization(policy =>
        {
            foreach (string permission in permissions)
            {
                policy.AddPolicy(
                    permission,
                    policy => policy.Requirements.Add(new PermissionRequirement(permission)));
            }
        });

        return services;
    }

    // That's kinda stupid...
    private static string[] GetPermissions()
    {
        string[] permissions =
        [
            "product:read",
            "order:write",
            "order:cancel",
            "product:write",
            "product:update",
            "product:delete",
        ];

        return permissions;
    }
}