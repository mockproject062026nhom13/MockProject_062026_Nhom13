using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.CareLevelResidents.DTOs;

namespace NursingHome.Application.Features.CareLevelResidents.Commands;

public class CreateAssessmentCommandHandler
    : IRequestHandler<
        CreateAssessmentCommand,
        ApiResponse<CreateAssessmentResponse>>
{
    private readonly IAssessmentService _assessmentService;

    public CreateAssessmentCommandHandler(
        IAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;
    }

    public async Task<ApiResponse<CreateAssessmentResponse>> Handle(
        CreateAssessmentCommand request,
        CancellationToken cancellationToken)
    {
        var assessmentId =
            await _assessmentService.CreateAssessmentAsync(
                request.Assessment,
                cancellationToken);


        return ApiResponse<CreateAssessmentResponse>.CreateSuccess(
            new CreateAssessmentResponse
            {
                AssessmentId = assessmentId,
                ResidentId = request.Assessment.ResidentId,
                AdlTotalScore =
                    request.Assessment.AssessmentDetails.Sum(x => x.Score),
                CreatedAt = DateTimeOffset.UtcNow
            },
            200,
            "Assessment created successfully.");
    }
}