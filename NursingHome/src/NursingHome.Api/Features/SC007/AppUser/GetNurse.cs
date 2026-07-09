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
        var nurseNames = await db.Users
            .Where(u => u.Status == "active" && !u.IsDeleted && u.RoleId == 1)
            .Select(u => u.FirstName + " " + u.LastName)
            .ToListAsync(ct);

        await SendAsync(ApiResponse<List<string>>.CreateSuccess(nurseNames), cancellation: ct);
    }
}
