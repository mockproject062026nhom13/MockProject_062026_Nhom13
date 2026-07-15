using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;

public record UpdateBedStatusRequest(long BedId);

public class UpdateBedStatusEndpoint(NursingHomeDbContext db) : Endpoint<UpdateBedStatusRequest, ApiResponse<bool>>
{
    public override void Configure()
    {
        Put("/api/beds/{BedId}/status");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateBedStatusRequest req, CancellationToken ct)
    {
        var bed = await db.Beds.FirstOrDefaultAsync(b => b.Id == req.BedId, ct);

        if (bed == null)
        {
            await SendAsync(ApiResponse<bool>.CreateError(404, "Bed not found"), 404, ct);
            return;
        }

        if (bed.Status == "Occupied")
        {
            await SendAsync(ApiResponse<bool>.CreateError(400, "Bed is already occupied"), 400, ct);
            return;
        }

        db.Entry(bed).Property(b => b.Status).CurrentValue = "Occupied";
        await db.SaveChangesAsync(ct);

        await SendAsync(ApiResponse<bool>.CreateSuccess(true), 200, ct);
    }
}
