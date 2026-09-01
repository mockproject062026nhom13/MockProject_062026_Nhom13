using System.Threading;
using System.Threading.Tasks;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

namespace NursingHome.Application.Abstractions;

public interface IAdmissionRepository
{
    Task<PreAdmissionScreeningDetailDto?> GetPreAdmissionScreeningDetailAsync(
        long screeningId,
        CancellationToken cancellationToken);
}
