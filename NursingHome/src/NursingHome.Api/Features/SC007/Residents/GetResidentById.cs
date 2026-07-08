using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Application.Common;

public record ResidentRequest(Guid Id);

public record ResidentResponse(
    string FullName,
    DateOnly DoB,
    string? Referral);

public class GetResidentByIdEndpoint(NursingHomeDbContext db) :
    Endpoint<ResidentRequest, ApiResponse<ResidentResponse>>
{
    public override void Configure()
    {
        Get("api/residents/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ResidentRequest req, CancellationToken ct)
    {
        var resident = await db.residents
            .Select(r => new ResidentResponse(
                r.first_name + " " + r.last_name,
                r.date_of_birth,
              r.religion_preference
            ))
            .FirstOrDefaultAsync(ct);

        if (resident is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(ApiResponse<ResidentResponse>.CreateSuccess(resident), cancellation: ct);
    }
}
