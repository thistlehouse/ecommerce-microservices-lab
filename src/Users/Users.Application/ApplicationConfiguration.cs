using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Users.Application.Common.Behaviours;

namespace Users.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblies(typeof(ApplicationConfiguration).Assembly));

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehaviour<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
