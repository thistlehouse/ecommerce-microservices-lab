using Products.Api;
using Products.Api.Extensions;
using Products.Application;
using Products.Infrastructure;

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
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ApplyMigrations();
app.UseExceptionHandler();
app.MapEndpoints();
app.UseHttpsRedirection();

app.Run();
