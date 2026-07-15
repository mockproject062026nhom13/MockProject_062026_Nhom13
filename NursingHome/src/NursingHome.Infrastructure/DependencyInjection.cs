using Microsoft.Extensions.DependencyInjection;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Repositories.UserSecurity;
using NursingHome.Infrastructure.Persistence.Repositories.CareLevelResidents;
using NursingHome.Infrastructure.Persistence.Repositories.RiskAuditLogs;
using NursingHome.Application.Abstractions.Auth;
using NursingHome.Application.Abstractions.RiskAuditLogs;
using NursingHome.Infrastructure.Persistence.Repositories.Auth;
using NursingHome.Infrastructure.Services;

namespace NursingHome.Infrastructure;

public static class DependencyInjection{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        //services.AddScoped<IUserRepository,UserRepository>();

        services.AddScoped<
            NursingHome.Application.Abstractions.IUserRepository,
            NursingHome.Infrastructure.Repositories.UserSecurity.UserRepository>();

        services.AddScoped<
            NursingHome.Application.Abstractions.Auth.IUserRepository,
            NursingHome.Infrastructure.Persistence.Repositories.Auth.UserRepository>();
        services.AddMemoryCache();
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IIncidentSeverityRepository, IncidentSeverityRepository>();
        services.AddScoped<ICareLevelResidentRepository, CareLevelResidentRepository>();
        services.AddScoped<IIncidentRepository, IncidentRepository>();
        return services;
    }

}