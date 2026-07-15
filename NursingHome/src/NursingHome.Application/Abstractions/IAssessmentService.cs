using NursingHome.Application.Features.CareLevelResidents.DTOs;
namespace NursingHome.Application.Abstractions;

public interface IAssessmentService
{
    // because this one is 1 use case <=> 1 transaction
    Task<long> CreateAssessmentAsync(
        AssessmentCreateDTO dto,
        CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}