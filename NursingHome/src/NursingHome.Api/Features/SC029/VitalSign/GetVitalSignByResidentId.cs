using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Api.Features.VitalSign;


public class GetLatestVitalSignEndpoint(NursingHomeDbContext db)
  : EndpointWithoutRequest<VitalSignDto>
{
    public override void Configure()
    {
        Get("/api/vital-signs/{ResidentId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var residentId = Route<long>("ResidentId");

        var latestVital = await db.VitalSigns
            .Include(v => v.RecordedByNavigation)
            .Where(v => v.ResidentId == residentId)
            .OrderByDescending(v => v.RecordedAt)
            .Select(v => new VitalSignDto(
                v.RecordedByNavigation.FirstName + " " + v.RecordedByNavigation.LastName,
                v.Spo2Percentage,
                v.RecordedAt
            ))
            .FirstOrDefaultAsync(ct);

        if (latestVital is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(latestVital, 200, ct);
    }
}
