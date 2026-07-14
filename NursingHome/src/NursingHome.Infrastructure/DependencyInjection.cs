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

namespace NursingHome.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NursingHomeDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(NursingHomeDbContext).Assembly.FullName)));

        services.AddScoped<IUserRepository, UserRepository>();


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
