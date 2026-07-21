using NursingHome.Application.Features.CarePlans.DTOs;

namespace NursingHome.Application.Abstractions.CarePlans;

public interface ICarePlanRepository
{
    Task<bool> ResidentExistsAsync(long residentId, CancellationToken cancellationToken = default);
    Task<bool> IsResidentChartLockedAsync(long residentId, CancellationToken cancellationToken = default);

    Task<CarePlanDetailDto> CreateDraftAsync(
        long residentId, bool significantChangeFlag, IReadOnlyList<CareAreaInput> areas, CancellationToken cancellationToken = default);

    Task<CarePlanSnapshot?> GetSnapshotAsync(long planId, CancellationToken cancellationToken = default);

    Task<CarePlanDetailDto> ReplaceCareAreasAsync(
        long planId, IReadOnlyList<CareAreaInput> areas, CancellationToken cancellationToken = default);

    // Sinh care_tasks từ interventions, chuyển plan sang ACTIVE, trả số task đã tạo.
    Task<int> ActivateAsync(long planId, CancellationToken cancellationToken = default);
}
