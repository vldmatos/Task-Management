using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Configurations.Extensions;

public interface IEndpoint
{
    string Group { get; }

    void MapEndpoints(IEndpointRouteBuilder endpointRouteBuilder);
}

public static class Endpoints
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] endpointServiceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(endpointServiceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication webApplication, RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = webApplication.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder endpointRouteBuilder = routeGroupBuilder is null ? webApplication : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoints(endpointRouteBuilder);
        }

        return webApplication;
    }
}
