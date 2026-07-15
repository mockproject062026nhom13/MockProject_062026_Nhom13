using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        services.AddDbContext<NursingHomeDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(NursingHomeDbContext).Assembly.FullName)));

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
            .AddClasses(classes => classes.InNamespaces(
                "NursingHome.Infrastructure.Persistence.Repositories",
                "NursingHome.Infrastructure.Services"))
            .AsMatchingInterface()
            .WithScopedLifetime());

        services.AddMemoryCache();
        return services;

    }
}
