using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NursingHome.Application.Abstractions.Authentication;
using NursingHome.Application.Abstractions.Repositories;
using NursingHome.Infrastructure.Jobs;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Repositories.UserSecurity;
using NursingHome.Infrastructure.Services.Authentication;
using NursingHome.Infrastructure.Services.Messaging;
using NursingHome.Infrastructure.Services.Security;
using NursingHome.Application.Abstractions.Services;
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
        services.AddDbContext<NursingHomeDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(NursingHomeDbContext).Assembly.FullName)));
        
        // Current user & auditing
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<AuditSaveChangesInterceptor>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IActivateAccountRepository, ActivateAccountRepository>();
        services.AddScoped<ICareLevelResidentRepository, CareLevelResidentRepository>();
        services.AddScoped<ILOCRateRepository, LOCRateRepository>();
        services.AddScoped<IIncidentSeverityRepository, IncidentSeverityRepository>();
        services.AddScoped<IIncidentRepository, IncidentRepository>();

        // Services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddMemoryCache();
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<ITokenService, TokenService>();

        // DbContext
        services.AddDbContext<NursingHomeDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));

            options.AddInterceptors(
                sp.GetRequiredService<AuditSaveChangesInterceptor>());
        });


        services.AddHostedService<StaffingComplianceBackgroundJob>();

        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

        services.AddMemoryCache();
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IStaffingRuleService, StaffingRuleService>();
        services.AddScoped<ICnaDashboardService, CnaDashboardService>();

        return services;
    }
}
