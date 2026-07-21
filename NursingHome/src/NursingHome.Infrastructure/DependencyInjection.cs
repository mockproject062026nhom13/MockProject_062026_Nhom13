using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NursingHome.Infrastructure.Authorization;
using NursingHome.Infrastructure.Jobs;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Services;
using NursingHome.Infrastructure.Persistence.Audit;


namespace NursingHome.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditSaveChangesInterceptor>();

        services.AddMemoryCache();
        services.AddAuthorization();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        // DbContext
        services.AddDbContext<NursingHomeDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(NursingHomeDbContext).Assembly.FullName));

            options.AddInterceptors(
                sp.GetRequiredService<AuditSaveChangesInterceptor>());
        });


        services.AddHostedService<StaffingComplianceBackgroundJob>();

        services.Scan(scan => scan
            .FromAssemblyOf<TokenService>()
            .AddClasses(classes => classes.AssignableTo<IAuthorizationHandler>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.Where(type =>
                type.Namespace != null &&
                (type.Namespace.StartsWith("NursingHome.Infrastructure.Persistence.Repositories") ||
                 type.Namespace.StartsWith("NursingHome.Infrastructure.Services") ||
                 type.Namespace.StartsWith("NursingHome.Infrastructure.Authorization"))))
            .AsMatchingInterface()
            .WithScopedLifetime());

        services.AddMemoryCache();
        return services;

    }
}
