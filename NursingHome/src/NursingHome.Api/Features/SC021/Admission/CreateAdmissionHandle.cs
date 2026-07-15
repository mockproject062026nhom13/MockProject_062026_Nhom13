using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Api.Features.CreateAdmission;



public class CreateAdmissionEndPoint(NursingHomeDbContext db)
: Endpoint<CreateAdmissionCommand, ApiResponse<bool>>
{
    public override void Configure()
    {
        Post("/api/admission");
        AllowAnonymous();
    }
    public override async Task HandleAsync(CreateAdmissionCommand req, CancellationToken ct)
    {
        using var transaction = await db.Database.BeginTransactionAsync(ct);

        try
        {
            await UpdateBedStatus(req.BedId, ct);
            await CreateNewAdmission(req, ct);

            // Gọi tiếp các nghiệp vụ khác ở đây (ví dụ: SaveAdmission, NotifyResident...)
            // await SaveAdmission(req, ct);
            await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            await SendAsync(ApiResponse<bool>.CreateSuccess(true), 200, ct);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            await SendAsync(ApiResponse<bool>.CreateError(400, ex.Message), 400, ct);
        }
    }

    private async Task UpdateBedStatus(long bedId, CancellationToken ct)
    {
        var bed = await db.Beds.FirstOrDefaultAsync(b => b.Id == bedId, ct);

        if (bed == null || bed.Status == "Occupied")
        {
            throw new Exception("Bed not available");
        }
        db.Entry(bed).Property(b => b.Status).CurrentValue = "Occupied";

    }
    private async Task CreateNewAdmission(CreateAdmissionCommand req, CancellationToken ct)
    {
        var admission = new Admission();
        var entry = db.Entry(admission);

        entry.Property("AdmissionDate").CurrentValue = req.AdmissionDate;
        entry.Property("ResidentId").CurrentValue = req.ResidentId;
        entry.Property("FacilityId").CurrentValue = req.FacilityId;
        entry.Property("CreatedAt").CurrentValue = DateTimeOffset.UtcNow;

        await db.Admissions.AddAsync(admission, ct);
    }
}
