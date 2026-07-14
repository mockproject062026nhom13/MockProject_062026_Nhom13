using Microsoft.Extensions.DependencyInjection;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Repositories.UserSecurity;
using NursingHome.Infrastructure.Persistence.Repositories.CareLevelResidents;

namespace NursingHome.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<ICareLevelResidentRepository, CareLevelResidentRepository>();

        return services;
    }
}