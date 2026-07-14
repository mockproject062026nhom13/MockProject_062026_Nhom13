using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

namespace NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

public class GetPreAdmissionScreeningDetailQueryHandler
    : IRequestHandler<GetPreAdmissionScreeningDetailQuery, ApiResponse<PreAdmissionScreeningDetailDto>>
{
    private readonly IAdmissionRepository _admissionRepository;

    public GetPreAdmissionScreeningDetailQueryHandler(IAdmissionRepository admissionRepository)
    {
        _admissionRepository = admissionRepository;
    }

    public async Task<ApiResponse<PreAdmissionScreeningDetailDto>> Handle(
        GetPreAdmissionScreeningDetailQuery request,
        CancellationToken cancellationToken)
    {
        var screening = await _admissionRepository.GetPreAdmissionScreeningDetailAsync(
            request.ScreeningId,
            cancellationToken);

        if (screening is null)
        {
            return ApiResponse<PreAdmissionScreeningDetailDto>.CreateError(
                404,
                "The requested pre-admission screening was not found.");
        }

        return ApiResponse<PreAdmissionScreeningDetailDto>.CreateSuccess(screening);
    }
}
