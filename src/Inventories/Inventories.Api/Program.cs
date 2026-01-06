using Inventories.Api;
using Inventories.Api.Extensions;
using Inventories.Application;
using Inventories.Infrastructure;
using Inventories.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation(builder.Configuration)
    .AddEndpoints()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using IServiceScope scope = app.Services.CreateScope();
InventoryDbContext context = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
context.Database.Migrate();

app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();

app.Run();