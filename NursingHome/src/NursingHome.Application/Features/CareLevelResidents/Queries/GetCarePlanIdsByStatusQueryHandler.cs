using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;

namespace NursingHome.Application.Features.CareLevelResidents.Queries.GetCarePlanIdsByStatus;

public sealed class GetCarePlanIdsByStatusQueryHandler
    : IRequestHandler<GetCarePlanIdsByStatusQuery, ApiResponse<List<long>>>
{
    private readonly ICarePlanRepository _carePlanRepository;

    public GetCarePlanIdsByStatusQueryHandler(
        ICarePlanRepository carePlanRepository)
    {
        _carePlanRepository = carePlanRepository;
    }

    public async Task<ApiResponse<List<long>>> Handle(
        GetCarePlanIdsByStatusQuery request,
        CancellationToken cancellationToken)
    {
        var carePlanIds = await _carePlanRepository.GetCarePlanIdByStatus(
            request.Status,
            cancellationToken);

        if (carePlanIds == null || carePlanIds.Count == 0)
        {
            return new ApiResponse<List<long>>
            {
                Success = false,
                Message = "No care plans found with this status",
                Data = []
            };
        }

        return new ApiResponse<List<long>>
        {
            Success = true,
            Message = "Get care plan ids successfully",
            Data = carePlanIds
        };
    }
}