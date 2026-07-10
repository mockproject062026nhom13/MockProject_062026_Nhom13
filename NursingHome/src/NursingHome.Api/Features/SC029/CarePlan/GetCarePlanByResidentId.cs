using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;
using Dapper;


namespace NursingHome.Api.Features.CarePlan;
public record GetRequest(
    long ResidentId,
    long FacilityId
    );

public class GetCarePlanEndpoint(NursingHomeDbContext db)
  : Endpoint<GetRequest, ApiResponse<List<CarePlanDto>>>
{
    public override void Configure()
    {
        Get("/api/care-plan/{ResidentId}/{FacilityId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetRequest req, CancellationToken ct)
    {
        var connection = db.Database.GetDbConnection();
        const string sql = @"
        SELECT 
            clr.care_level_id AS CareLevelId,
            clr.daily_rate AS DailyRate,
            ro.room_number AS RoomNumber,
            b.bed_number AS BedNumber,
            ic.category_name AS CareName,
            ic.description AS Description,
            rclh.start_date AS StartDate
        FROM (
            SELECT *, 
                   ROW_NUMBER() OVER(PARTITION BY resident_id ORDER BY id DESC) as rn
            FROM resident_care_level_history
        ) rclh
        JOIN (
            SELECT *,
                   ROW_NUMBER() OVER(PARTITION BY care_level_id, facility_id ORDER BY id DESC) as rn_rate
            FROM care_level_rates
            WHERE facility_id = @FacilityId
        ) clr ON clr.care_level_id = rclh.care_level_id AND clr.rn_rate = 1
        JOIN residents r ON r.id = rclh.resident_id
        JOIN beds b ON b.id = r.bed_id
        JOIN rooms ro ON ro.id = b.room_id
        JOIN durable_medical_equipment dme ON dme.assigned_to_resident = rclh.resident_id
        JOIN inventory_categories ic ON ic.id = dme.category_id
        WHERE rclh.rn = 1 AND rclh.resident_id = @ResidentId";
        var data = (await connection.QueryAsync<CarePlanDto>(sql, new
        {
            req.FacilityId,
            req.ResidentId
        })).ToList();

        var response = ApiResponse<List<CarePlanDto>>.CreateSuccess(data);
        await SendAsync(response, 200, ct);
    }
}
