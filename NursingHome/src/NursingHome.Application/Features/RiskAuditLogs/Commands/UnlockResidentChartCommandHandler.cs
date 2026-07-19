using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Features.RiskAuditLogs.Commands;

public sealed class UnlockResidentChartCommandHandler
    : IRequestHandler<UnlockResidentChartCommand, ApiResponse<UnlockResidentChartResultDto>>
{
    private readonly IChartLockRepository _chartLockRepository;
    private readonly ICurrentUserService _currentUserService;

    public UnlockResidentChartCommandHandler(
        IChartLockRepository chartLockRepository,
        ICurrentUserService currentUserService)
    {
        _chartLockRepository = chartLockRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResponse<UnlockResidentChartResultDto>> Handle(
        UnlockResidentChartCommand request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not long currentUserId)
        {
            return ApiResponse<UnlockResidentChartResultDto>.CreateError(
                401,
                "You are not authorized to perform this action.");
        }

        var result = await _chartLockRepository.UnlockResidentChartAsync(
            request.IncidentId,
            currentUserId,
            request.Reason!.Trim(),
            cancellationToken);

        return result.Status switch
        {
            ChartUnlockOperationStatus.Success => ApiResponse<UnlockResidentChartResultDto>.CreateSuccess(
                result.Data,
                message: "Resident chart unlocked successfully."),

            ChartUnlockOperationStatus.NotFound => ApiResponse<UnlockResidentChartResultDto>.CreateError(
                404,
                "The requested incident was not found."),

            ChartUnlockOperationStatus.AlreadyUnlocked => ApiResponse<UnlockResidentChartResultDto>.CreateError(
                409,
                "The resident chart is already unlocked."),

            _ => ApiResponse<UnlockResidentChartResultDto>.CreateError(
                500,
                "An unexpected error occurred while processing your request.")
        };
    }
}
