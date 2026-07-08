using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Application.Common;

public record RoleResponse(
    string Name,
    string? Description);

public class GetRoleByUserIdEndpoint(NursingHomeDbContext db) :
    EndpointWithoutRequest<ApiResponse<RoleResponse>>
{
    public override void Configure()
    {
        Get("api/users/{UserId}/role");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = Route<Guid>("UserId");

        var role = await db.users
            .Where(u => u.id == userId)
            .Select(u => u.role)
            .Select(r => new RoleResponse(
                r.role_name,
                r.description
            ))
            .FirstOrDefaultAsync(ct);

        if (role is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(ApiResponse<RoleResponse>.CreateSuccess(role), cancellation: ct);
    }
}
