using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NursingHome.Application.Abstractions.Authentication;
using NursingHome.Application.Abstractions.Repositories;
using NursingHome.Infrastructure.Jobs;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Repositories.EmarShift;
using NursingHome.Infrastructure.Persistence.Repositories.UserSecurity;
using NursingHome.Infrastructure.Services.Authentication;

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
        services.AddScoped<IStaffingConfigRepository, StaffingConfigRepository>();
        services.AddScoped<IStaffingComplianceRepository, StaffingComplianceRepository>();


        services.AddHostedService<StaffingComplianceBackgroundJob>();

        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

        return services;
    }
}
