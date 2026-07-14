using Microsoft.Extensions.DependencyInjection;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Repositories.UserSecurity;
using NursingHome.Infrastructure.Persistence.Repositories.CareLevelResidents;
using NursingHome.Infrastructure.Persistence.Repositories.RiskAuditLogs;

namespace NursingHome.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        //SC_005-AD-02_create-edit-user
        services.AddScoped<IUserRepository, UserRepository>();

        //SC_017-M1-US-01_resident-list
        services.AddScoped<ICareLevelResidentRepository, CareLevelResidentRepository>();

        //SC_037_M7-US-01_report_incident
        services.AddScoped<IIncidentRepository, IncidentRepository>();

        return services;
    }
}