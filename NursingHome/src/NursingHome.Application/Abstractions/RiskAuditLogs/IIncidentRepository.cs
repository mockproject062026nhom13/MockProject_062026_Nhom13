
using NursingHome.Application.Features.RiskAuditLogs.DTOs;
using NursingHome.Application.Common.Models;
using NursingHome.Application.Features.RiskAuditLogs.Commands;

namespace NursingHome.Application.Abstractions.RiskAuditLogs;

public interface IIncidentRepository
{
    Task<PageResult<IncidentListItemDto>> GetPagedIncidentsAsync(
        string statusFilter,
        string severityFilter,
        int pageIndex, 
        int pageSize,
        CancellationToken cancellationToken

    );

    Task<IncidentSummaryDto>GetIncidentSummaryAsync(
        int month,
        int year,
        CancellationToken cancellationToken
    );

    Task<long> CreateIncidentAsync(CreateIncidentCommand command, long currentUserId, CancellationToken cancellationToken);
}