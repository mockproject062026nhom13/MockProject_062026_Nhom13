using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Features.RiskAuditLogs.Commands;

public sealed class UnlockResidentChartCommand : IRequest<ApiResponse<UnlockResidentChartResultDto>>
{
    public long IncidentId { get; set; }

    public string? Reason { get; set; }
}
