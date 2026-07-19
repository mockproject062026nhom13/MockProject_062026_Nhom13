namespace NursingHome.Application.Features.RiskAuditLogs.DTOs;

public sealed class IncidentChartLockDetailDto
{
    public IncidentChartLockIncidentDto Incident { get; set; } = null!;

    public IncidentChartLockSeverityDto Severity { get; set; } = null!;

    public IncidentChartLockResidentDto Resident { get; set; } = null!;

    public List<IncidentChartLockTimelineItemDto> Timeline { get; set; } = [];
}

public sealed class IncidentChartLockIncidentDto
{
    public long IncidentId { get; set; }

    public string Status { get; set; } = string.Empty;

    public string IncidentType { get; set; } = string.Empty;

    public DateTimeOffset ReportedAt { get; set; }

    public string Description { get; set; } = string.Empty;
}

public sealed class IncidentChartLockSeverityDto
{
    public long SeverityId { get; set; }

    public string LevelName { get; set; } = string.Empty;

    public bool ChartLockTrigger { get; set; }
}

public sealed class IncidentChartLockResidentDto
{
    public long ResidentId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public bool IsChartLocked { get; set; }
}

public sealed class IncidentChartLockTimelineItemDto
{
    public long TimelineId { get; set; }

    public string? Action { get; set; }

    public string? Reason { get; set; }

    public long? ActorId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
