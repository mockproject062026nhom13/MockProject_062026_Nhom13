// using Microsoft.Extensions.DependencyInjection;

// namespace NursingHome.Api.Extensions;

// public static class DependencyInjection
// {
//     public static IServiceCollection AddCustomAuthorizationPolicies(this IServiceCollection services)
//     {
//         services.AddAuthorization(options =>
//         {
            
//             options.AddPolicy("CanViewIncidentDashboard", policy =>
//             {
//                 policy.RequireAuthenticatedUser();
//                 policy.RequireClaim("Permission", "INCIDENT_LIST_VIEW");
//             });
//         });

        
//         return services;
//     }
// }