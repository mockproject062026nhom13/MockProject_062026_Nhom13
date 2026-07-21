using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Features.RiskAuditLogs.Queries;

public sealed class GetIncidentChartLockQuery : IRequest<ApiResponse<IncidentChartLockDetailDto>>
{
    public long IncidentId { get; set; }
}
