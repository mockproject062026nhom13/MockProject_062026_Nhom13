using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Features.RiskAuditLogs.Queries;

public sealed class GetIncidentChartLockQueryHandler
    : IRequestHandler<GetIncidentChartLockQuery, ApiResponse<IncidentChartLockDetailDto>>
{
    private readonly IChartLockRepository _chartLockRepository;

    public GetIncidentChartLockQueryHandler(IChartLockRepository chartLockRepository)
    {
        _chartLockRepository = chartLockRepository;
    }

    public async Task<ApiResponse<IncidentChartLockDetailDto>> Handle(
        GetIncidentChartLockQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _chartLockRepository.GetIncidentChartLockAsync(
            request.IncidentId,
            cancellationToken);

        if (result is null)
        {
            return ApiResponse<IncidentChartLockDetailDto>.CreateError(
                404,
                "The requested incident was not found.");
        }

        return ApiResponse<IncidentChartLockDetailDto>.CreateSuccess(result);
    }
}
