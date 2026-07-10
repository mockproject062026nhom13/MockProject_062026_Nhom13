using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;

public record AvailableBedRecord(
    long BedId,
    string RoomNumber,
    string BedNumber
);

public class GetAvailableBedsEndpoint(NursingHomeDbContext db) : EndpointWithoutRequest<ApiResponse<List<AvailableBedRecord>>>
{
    public override void Configure()
    {
        Get("/api/beds/available");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var data = await db.Beds
            .Where(b => b.Status == "Available")
            .Select(b => new AvailableBedRecord(
                b.Id,
                b.Room.RoomNumber,
                b.BedNumber
            ))
            .ToListAsync(ct);

        await SendAsync(ApiResponse<List<AvailableBedRecord>>.CreateSuccess(data), cancellation: ct);
    }
}
