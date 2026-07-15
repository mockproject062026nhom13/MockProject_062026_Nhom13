using MediatR;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Features.RiskAuditLogs.Queries;
public record GetIncidentSummaryQuery(
    int Month,
    int Year
):IRequest<IncidentSummaryDto>;