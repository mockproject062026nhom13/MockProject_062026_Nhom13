using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Persistence.Audit;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Repositories.CareLevelResidents;
using NursingHome.Infrastructure.Repositories.UserSecurity;
using NursingHome.Infrastructure.Persistence.Repositories.LocationInfrastructure;

namespace NursingHome.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Current user & auditing
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<AuditSaveChangesInterceptor>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IActivateAccountRepository, ActivateAccountRepository>();
        services.AddScoped<ICareLevelResidentRepository, CareLevelResidentRepository>();
        services.AddScoped<ILOCRateRepository, LOCRateRepository>();

        // Services
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // DbContext
        services.AddDbContext<NursingHomeDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));

            options.AddInterceptors(
                sp.GetRequiredService<AuditSaveChangesInterceptor>());
        });

        return services;
    }
}