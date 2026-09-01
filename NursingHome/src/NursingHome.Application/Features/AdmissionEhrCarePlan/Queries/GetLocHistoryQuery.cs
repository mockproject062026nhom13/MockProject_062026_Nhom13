using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;

namespace NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

public class GetLocHistoryQuery : IRequest<ApiResponse<LocHistoryResultDto>>
{
    public long ResidentId { get; set; }
}
