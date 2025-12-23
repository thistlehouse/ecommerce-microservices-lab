using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Users.Api.Endpoints;

namespace Users.Api.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndPoints(this IServiceCollection services)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        IEnumerable<Type>? endpointTypes = assembly
            .GetTypes()
            .Where(type =>
                type is { IsAbstract: false, IsInterface: false } &&
                type.IsAssignableTo(typeof(IEndpoint)));

        foreach (Type type in endpointTypes)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IEndpoint), type));
        }

        return services;
    }

    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        IEndpointRouteBuilder builder = routeBuilder is null ? app : routeBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}