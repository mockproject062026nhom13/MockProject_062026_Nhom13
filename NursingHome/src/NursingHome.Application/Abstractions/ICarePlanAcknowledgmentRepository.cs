using NursingHome.Application.Features.CarePlanAcknowledgments.DTOs;

namespace NursingHome.Application.Abstractions;

public interface ICarePlanAcknowledgmentRepository
{
    Task<CarePlanAcknowledgmentCoreDto?> GetCarePlanAcknowledgmentCoreAsync(
        long carePlanId,
        CancellationToken cancellationToken);

    Task<CarePlanCurrentUserDto?> GetCurrentUserAsync(
        long currentUserId,
        CancellationToken cancellationToken);
}
