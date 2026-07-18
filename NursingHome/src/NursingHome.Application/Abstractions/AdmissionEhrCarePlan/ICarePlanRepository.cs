using NursingHome.Application.Common.Models;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

namespace NursingHome.Application.Abstractions.AdmissionEhrCarePlan;

public interface ICarePlanRepository
{
    Task<PageResult<CarePlanDto>> GetCarePlanListAsync(
        string? searchTerm,
        string? status,
        string? reviewStatus,
        long currentUserId,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken
    );

    Task<CarePlanStatisticDto> GetSratisticsByNurseIdAsync(long nurseId,CancellationToken cancellationToken);
}
