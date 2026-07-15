
using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Abstractions;

public interface IIncidentRepository
{
    Task<List<IncidentListItemDto>> GetPagedIncidentsAsync(
        string statusFilter,
        string severityFilter,
        int skip,
        int take,
        CancellationToken cancellationToken

    );

    Task<IncidentSummaryDto>GetIncidentSummaryAsync(
        int month,
        int year,
        CancellationToken cancellationToken
    );
}