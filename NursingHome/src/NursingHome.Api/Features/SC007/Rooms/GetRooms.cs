using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;

public class GetBedsEndpoint(NursingHomeDbContext db) : EndpointWithoutRequest<ApiResponse<List<BedRecord>>>
{
    public override void Configure()
    {
        Get("/api/beds");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var data = await db.beds
            .Select(b => new BedRecord(
                b.room.room_number,
                b.room.room_type,
                b.bed_number,
                b.status
            ))
            .ToListAsync(ct);
        var response = ApiResponse<List<BedRecord>>.CreateSuccess(data);
        await SendAsync(response, cancellation: ct);
    }
}


