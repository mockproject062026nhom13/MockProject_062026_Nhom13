using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.CarePlanAcknowledgments.DTOs;

namespace NursingHome.Application.Features.CarePlanAcknowledgments.Queries;

public class GetCarePlanAcknowledgmentQueryHandler
    : IRequestHandler<GetCarePlanAcknowledgmentQuery, ApiResponse<CarePlanAcknowledgmentDto>>
{
    private readonly ICarePlanAcknowledgmentRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public GetCarePlanAcknowledgmentQueryHandler(
        ICarePlanAcknowledgmentRepository repository,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResponse<CarePlanAcknowledgmentDto>> Handle(
        GetCarePlanAcknowledgmentQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not long currentUserId)
        {
            return ApiResponse<CarePlanAcknowledgmentDto>.CreateError(
                401,
                "You are not authorized to perform this action.");
        }

        var core = await _repository.GetCarePlanAcknowledgmentCoreAsync(
            request.CarePlanId,
            cancellationToken);

        if (core is null)
        {
            return ApiResponse<CarePlanAcknowledgmentDto>.CreateError(
                404,
                "The requested care plan was not found.");
        }

        var currentUser = await _repository.GetCurrentUserAsync(
            currentUserId,
            cancellationToken);

        if (currentUser is null)
        {
            return ApiResponse<CarePlanAcknowledgmentDto>.CreateError(
                404,
                "The current user was not found.");
        }

        return ApiResponse<CarePlanAcknowledgmentDto>.CreateSuccess(
            new CarePlanAcknowledgmentDto
            {
                CarePlan = core.CarePlan,
                Resident = core.Resident,
                Goals = core.Goals,
                Interventions = core.Interventions,
                Tasks = core.Tasks,
                CurrentUser = currentUser
            });
    }
}
