using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

namespace NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

public class GetLocClassificationResultQuery
    : IRequest<ApiResponse<LocClassificationResultDto>>
{
    public long AssessmentId { get; set; }
}
