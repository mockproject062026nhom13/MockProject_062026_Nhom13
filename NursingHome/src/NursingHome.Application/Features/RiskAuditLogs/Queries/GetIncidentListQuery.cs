using MediatR;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Features.RiskAuditLogs.Queries;

public record GetIncidentListQuery(
    string StatusFilter,
    string SeverityFilter,
    int PageNumber,
    int PageSize
): IRequest<List<IncidentListItemDto>>;