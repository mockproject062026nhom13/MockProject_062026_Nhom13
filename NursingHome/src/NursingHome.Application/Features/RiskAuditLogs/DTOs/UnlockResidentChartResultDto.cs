namespace NursingHome.Application.Features.RiskAuditLogs.DTOs;

public sealed class UnlockResidentChartResultDto
{
    public long IncidentId { get; set; }

    public long ResidentId { get; set; }

    public bool IsChartLocked { get; set; }

    public DateTimeOffset UnlockedAt { get; set; }

    public long UnlockedByUserId { get; set; }

    public long TimelineId { get; set; }
}
