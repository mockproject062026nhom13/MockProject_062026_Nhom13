using NursingHome.Application.Features.RiskAuditLogs.DTOs;

namespace NursingHome.Application.Abstractions;

public interface IChartLockRepository
{
    Task<IncidentChartLockDetailDto?> GetIncidentChartLockAsync(
        long incidentId,
        CancellationToken cancellationToken);

    Task<ChartUnlockOperationResult> UnlockResidentChartAsync(
        long incidentId,
        long currentUserId,
        string reason,
        CancellationToken cancellationToken);
}
