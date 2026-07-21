using FastEndpoints;
using Dapper;
using NursingHome.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Features.CareLevelResidents.DTOs;

public class GetAssessmentsCommandHandler(NursingHomeDbContext db)
  : ICommandHandler<GetAssessmentsCommand, IEnumerable<ResidentAssessmentDto>>
{
    public async Task<IEnumerable<ResidentAssessmentDto>> ExecuteAsync(GetAssessmentsCommand command, CancellationToken ct)
    {
        var sql = @"
        WITH RankedAssessments AS (
            SELECT  
                a.id, a.resident_id, a.created_at, 
                r.first_name + ' ' + r.last_name AS fullname,
                b.bed_number, room.room_number,
                ROW_NUMBER() OVER (PARTITION BY a.resident_id ORDER BY a.id DESC) as rn,
                COUNT(*) OVER (PARTITION BY a.resident_id) as total_count
            FROM assessments a 
            JOIN users u ON u.id = a.assessed_by 
            JOIN residents r ON r.id = a.resident_id 
            LEFT JOIN beds b ON b.id = r.bed_id 
            LEFT JOIN rooms room ON b.room_id = room.id 
            WHERE a.assessed_by = @UserId
        )
        SELECT 
           ra.resident_id AS ResidentId, ra.fullname, ra.created_at AS CreatedAt, 
           ra.bed_number AS BedNumber, ra.room_number AS RoomNumber,
           CASE WHEN ra.total_count > 1 THEN 'Reassessment' ELSE 'Assessment' END AS AssessmentType,
           i.incident_type AS IncidentType, i.reported_at AS ReportedAt, t.level_name AS LevelName
        FROM RankedAssessments ra
        LEFT JOIN (
            SELECT resident_id, incident_type, reported_at, severity_id 
            FROM incidents 
            WHERE status != 'closed'
        ) i ON i.resident_id = ra.resident_id
        LEFT JOIN incident_severities t on i.severity_id = t.id
        WHERE ra.rn = 1";

        var dictionary = new Dictionary<int, ResidentAssessmentDto>();
        var connection = db.Database.GetDbConnection();

        await connection.QueryAsync<ResidentAssessmentDto, IncidentDto?, ResidentAssessmentDto>(
            sql,
            (res, inc) =>
            {
                if (!dictionary.TryGetValue(res.ResidentId, out var currentRes))
                {
                    currentRes = res;
                    dictionary.Add(currentRes.ResidentId, currentRes);
                }

                if (inc != null)
                {
                    currentRes.Incidents.Add(inc);
                }

                return currentRes;
            },
            param: new { command.UserId },
            splitOn: "IncidentType"
        );

        return dictionary.Values;
    }
}
