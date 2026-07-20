namespace NursingHome.Application.Features.RiskAuditLogs.DTOs;

public record IncidentSeverityDto(
    long Id,
    string LevelName,
    string? Description,
    string? Example
);