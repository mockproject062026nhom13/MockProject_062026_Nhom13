using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

namespace NursingHome.Application.Abstractions;

public interface ILocClassificationRepository
{
    Task<LocClassificationResultDto?> GetLocClassificationResultAsync(
        long assessmentId,
        CancellationToken cancellationToken);

    Task<LocHistoryResultDto?> GetLocHistoryAsync(
        long residentId,
        CancellationToken cancellationToken);
}
