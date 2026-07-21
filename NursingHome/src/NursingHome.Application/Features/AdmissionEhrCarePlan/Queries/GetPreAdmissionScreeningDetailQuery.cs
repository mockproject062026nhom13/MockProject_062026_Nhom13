using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

namespace NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

public class GetPreAdmissionScreeningDetailQuery
    : IRequest<ApiResponse<PreAdmissionScreeningDetailDto>>
{
    public long ScreeningId { get; set; }
}
