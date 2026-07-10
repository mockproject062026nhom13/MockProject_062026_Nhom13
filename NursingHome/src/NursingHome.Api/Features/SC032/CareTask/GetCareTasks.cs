using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Api.Features.CareTasks;

public class GetCareTasksEndpoint(NursingHomeDbContext db)
    : EndpointWithoutRequest<ApiResponse<List<CareTaskDto>>>
{
    public override void Configure()
    {
        Get("/api/care-tasks");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var tasks = await db.CareTasks
    .Where(t => t.CareIntervention.CarePlan.Status == "Active")
    .Select(t => new
    {
        Task = t,
        Resident = t.CareIntervention.CarePlan.Resident
    })
    .Select(t => new CareTaskDto(
        t.Task.TaskType,
        t.Task.Status,
        t.Task.ScheduledTime,
        t.Task.CareIntervention.CarePlan.Status,
        (t.Resident.FirstName + " " + t.Resident.LastName).Trim(),
        t.Resident.Bed != null ? t.Resident.Bed.Room.RoomNumber : "N/A",
        t.Resident.Bed != null ? t.Resident.Bed.BedNumber : "N/A"
    ))
    .ToListAsync(ct);
        await SendAsync(ApiResponse<List<CareTaskDto>>.CreateSuccess(tasks), 200, ct);
    }
}
