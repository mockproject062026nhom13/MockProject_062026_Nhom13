using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.CareLevelResidents.DTOs;

namespace NursingHome.Application.Features.CareLevelResidents.Commands;

public class CreateAssessmentCommand
    : IRequest<ApiResponse<CreateAssessmentResponse>>
{
    public AssessmentCreateDTO Assessment { get; set; } = default!;
}