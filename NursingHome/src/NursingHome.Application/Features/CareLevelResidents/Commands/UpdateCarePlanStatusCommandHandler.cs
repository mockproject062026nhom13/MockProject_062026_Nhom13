using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.CareLevelResidents.DTOs;

namespace NursingHome.Application.Features.CareLevelResidents.Commands;

public class UpdateCarePlanStatusCommandHandler
    : IRequestHandler<
        UpdateCarePlanStatusCommand,
        ApiResponse<UpdateCarePlanStatusResponse>>
{
    private readonly ICarePlanRepository _carePlanRepository;

    public UpdateCarePlanStatusCommandHandler(
        ICarePlanRepository carePlanService)
    {
        _carePlanRepository = carePlanService;
    }

    public async Task<ApiResponse<UpdateCarePlanStatusResponse>> Handle(
        UpdateCarePlanStatusCommand request,
        CancellationToken cancellationToken)
    {
        await _carePlanRepository.UpdateStatusByDON(
            request.CarePlanId,
            request.Status,
            cancellationToken);

        return ApiResponse<UpdateCarePlanStatusResponse>.CreateSuccess(
            new UpdateCarePlanStatusResponse
            {
                CarePlanId = request.CarePlanId,
                Status = request.Status,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            200,
            "Care plan status updated successfully.");
    }
}