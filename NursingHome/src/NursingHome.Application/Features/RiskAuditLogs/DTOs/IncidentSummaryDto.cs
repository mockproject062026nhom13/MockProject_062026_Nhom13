namespace NursingHome.Application.Features.RiskAuditLogs.DTOs;

public record IncidentSummaryDto(
    int TotalThisMonth,
    int Open,
    int Overdue,
    int ChartLocked,
    int Resolved
);