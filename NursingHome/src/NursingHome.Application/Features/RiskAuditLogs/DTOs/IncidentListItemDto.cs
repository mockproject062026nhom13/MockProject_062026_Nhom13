namespace NursingHome.Application.Features.RiskAuditLogs.DTOs;

public record IncidentListItemDto(
    long Id,
    string ResidentNameAndRoom,
    string IncidentType,
    string SeverityLevel,
    string ReportedTime,
    string SlaCountdown,
    string Status,
    string ChartStatus
);