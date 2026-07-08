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
        var physicians = await db.users
            .Where(u => u.status == "active" && !u.is_deleted && u.role_id == 4)
            .Select(u => new PhysicianResponse(
                u.first_name + " " + u.last_name,
                u.license_number
            ))
            .ToListAsync(ct);

        await SendAsync(ApiResponse<List<PhysicianResponse>>.CreateSuccess(physicians), cancellation: ct);
    }
}
