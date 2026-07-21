using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.CareLevelResidents.DTOs;

namespace NursingHome.Application.Features.CareLevelResidents.Commands;

public class UpdateCarePlanStatusCommand
    : IRequest<ApiResponse<UpdateCarePlanStatusResponse>>
{
    public long CarePlanId { get; set; }

    public string Status { get; set; } = default!;

    
}