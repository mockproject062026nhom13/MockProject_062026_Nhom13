using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Api.Features.CareTasks;

public record GetCareTasksRequest(
    [property: FromRoute] long AssignedCnaId
);

public class GetCareTasksEndpoint(NursingHomeDbContext db)
    : Endpoint<GetCareTasksRequest, ApiResponse<List<CareTaskDto>>>
{
    public override void Configure()
    {
        Get("/api/care-tasks/{AssignedCnaId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetCareTasksRequest req, CancellationToken ct)
    {
        var tasks = await db.CareTasks
            .Where(t => t.AssignedCnaId == req.AssignedCnaId
                     && t.CareIntervention.CarePlan.Status == "Active")
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
