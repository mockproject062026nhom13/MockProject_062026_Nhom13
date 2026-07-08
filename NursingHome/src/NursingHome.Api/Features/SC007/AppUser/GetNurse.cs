using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Application.Common;

public class GetActiveNurseNamesEndpoint(NursingHomeDbContext db) :
    EndpointWithoutRequest<ApiResponse<List<string>>>
{
    public override void Configure()
    {
        Get("api/users/nurse");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var nurseNames = await db.users
            .Where(u => u.status == "active" && !u.is_deleted && u.role_id == 1)
            .Select(u => u.first_name + " " + u.last_name)
            .ToListAsync(ct);

        await SendAsync(ApiResponse<List<string>>.CreateSuccess(nurseNames), cancellation: ct);
    }
}
