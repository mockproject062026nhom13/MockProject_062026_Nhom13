using Microsoft.Extensions.DependencyInjection;
using NursingHome.Application.Abstractions.Auth;
using NursingHome.Application.Abstractions.RiskAuditLogs;
using NursingHome.Infrastructure.Persistence.Repositories.Auth;
using NursingHome.Infrastructure.Services;

namespace NursingHome.Infrastructure;

public static class DependencyInjection{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddMemoryCache();
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IIncidentSeverityRepository, IncidentSeverityRepository>();
        services.AddScoped<ICareLevelResidentRepository, CareLevelResidentRepository>();
        return services;
    }

}