using Microsoft.Extensions.DependencyInjection;
using NursingHome.Infrastructure.Services;

namespace NursingHome.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TokenService>()
            .AddClasses(classes => classes.InNamespaces(
                "NursingHome.Infrastructure.Persistence.Repositories",
                "NursingHome.Infrastructure.Services"))
            .AsMatchingInterface()
            .WithScopedLifetime());

        services.AddMemoryCache();
        return services;
    }

}