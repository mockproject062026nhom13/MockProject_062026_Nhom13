using MediatR;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;
using NursingHome.Application.Common.Models;


namespace NursingHome.Application.Features.RiskAuditLogs.Queries;

public record GetIncidentListQuery(
    string StatusFilter,
    string SeverityFilter,
    int PageNumber,
    int PageSize
): IRequest<PageResult<IncidentListItemDto>>;