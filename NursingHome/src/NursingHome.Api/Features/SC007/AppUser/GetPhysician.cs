using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Application.Common;

public record PhysicianResponse(string FullName, string? License);

public class GetActivePhysicianNamesEndpoint(NursingHomeDbContext db) :
    EndpointWithoutRequest<ApiResponse<List<PhysicianResponse>>>
{
    public override void Configure()
    {
        Get("api/users/physician");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var physicians = await db.Users
            .Where(u => u.Status == "active" && !u.IsDeleted && u.RoleId == 4)
            .Select(u => new PhysicianResponse(
                u.FirstName + " " + u.LastName,
                u.LicenseNumber
            ))
            .ToListAsync(ct);

        await SendAsync(ApiResponse<List<PhysicianResponse>>.CreateSuccess(physicians), cancellation: ct);
    }
}
