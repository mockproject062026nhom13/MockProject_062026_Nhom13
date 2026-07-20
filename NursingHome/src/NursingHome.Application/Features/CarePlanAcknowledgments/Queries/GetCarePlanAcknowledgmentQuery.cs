using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.CarePlanAcknowledgments.DTOs;

namespace NursingHome.Application.Features.CarePlanAcknowledgments.Queries;

public class GetCarePlanAcknowledgmentQuery
    : IRequest<ApiResponse<CarePlanAcknowledgmentDto>>
{
    public long CarePlanId { get; set; }
}
