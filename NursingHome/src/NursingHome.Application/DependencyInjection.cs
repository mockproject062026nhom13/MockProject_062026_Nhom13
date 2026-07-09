using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NursingHome.Application.Behaviors;

namespace NursingHome.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register all MediatR handlers (IRequestHandler, INotificationHandler, ...) in this assembly
        // and hook the validation behavior into the request pipeline. Order matters: validation runs
        // before the handler executes.
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Register all FluentValidation validators (IValidator<T>) in this assembly.
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
