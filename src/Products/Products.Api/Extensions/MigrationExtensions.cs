using Microsoft.EntityFrameworkCore;
using Products.Infrastructure.Persistence;

namespace Products.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using ProductDbContext context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        context.Database.Migrate();
    }
}