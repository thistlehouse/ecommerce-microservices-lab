using Users.Api.Extensions;
using Users.Application;
using Users.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddEndPoints();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// using IServiceScope? scope = app.Services.CreateScope();
// UserDbContext context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
// context.Database.Migrate();

app.UseHttpsRedirection();

app.Use(async (ctx, next) =>
{
    Console.WriteLine($" -> Incoming Request {ctx.Request.Method} {ctx.Request.Path}");
    await next();
});

app.MapEndpoints();
app.Run();