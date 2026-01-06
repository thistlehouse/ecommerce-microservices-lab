using System.Text;
using Inventories.Api.Permissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Inventories.Api;

public static class PresentationConfiguration
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddAuthN(configuration)
            .AddAuthZ();

        return services;
    }

    private static IServiceCollection AddAuthN(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!)),
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Auth failed: " + context.Exception.Message);
                        // Log context.Exception for details (e.g., "IDX10501: Signature validation failed" or lifetime)
                        return Task.CompletedTask;
                    }
                };
            });


        return services;
    }

    private static IServiceCollection AddAuthZ(this IServiceCollection services)
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

    private static string[] GetPermissions()
    {
        string[] permissions =
        [
            "product:read",
            "order:cancel",
            "inventory:write",
            "inventory:read"
        ];

        return permissions;
    }
}