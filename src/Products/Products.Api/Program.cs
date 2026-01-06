using Microsoft.EntityFrameworkCore;
using Products.Api;
using Products.Api.Extensions;
using Products.Application;
using Products.Infrastructure;
using Products.Infrastructure.Persistence;
using Products.Infrastructure.Persistence.Seeds;
using Products.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation(builder.Configuration)
    .AddEndpoints()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient("Inventory", builder =>
{
    builder.BaseAddress = new Uri("http://localhost:5209");
})
.AddHttpMessageHandler<ServiceAuthenticationHandler>();

builder.Services.AddHttpClient("User", builder =>
{
    builder.BaseAddress = new Uri("http://localhost:5099");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using IServiceScope scope = app.Services.CreateScope();
ProductDbContext context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
context.Database.Migrate();

await Task.Delay(1000);

DatabaseInitialiser.Seed(context);

app.ApplyMigrations();
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.Use(async (ctx, next) =>
{
    Console.WriteLine($" -> Incoming request {ctx.Request.Method} {ctx.Request.Method}");
    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();

app.Run();
