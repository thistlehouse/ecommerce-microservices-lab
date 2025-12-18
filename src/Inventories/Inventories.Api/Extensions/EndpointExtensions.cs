using System.Reflection;
using Inventories.Api.Endpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Inventories.Api.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        IEnumerable<Type>? endpointTypes = assembly
            .GetTypes()
            .Where(type =>
                type is { IsAbstract: false, IsInterface: false } &&
                type.IsAssignableTo(typeof(IEndpoint)));

        foreach (Type endpoint in endpointTypes)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IEndpoint), endpoint));
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