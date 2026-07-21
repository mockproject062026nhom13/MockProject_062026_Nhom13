using MediatR;
using NursingHome.Application.Common; 
using NursingHome.Application.Features.CareLevelResidents.DTOs;
using NursingHome.Application.Abstractions;
namespace NursingHome.Application.Features.CareLevelResidents.Queries;

public sealed class GetCarePlanQueryHandler 
    : IRequestHandler<GetCarePlanQuery, ApiResponse<CarePlanInfoDto>>
{
    private readonly ICarePlanRepository _carePlanRepository;

    public GetCarePlanQueryHandler(
        ICarePlanRepository carePlanRepository)
    {
        _carePlanRepository = carePlanRepository;
    }

    public async Task<ApiResponse<CarePlanInfoDto>> Handle(
        GetCarePlanQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _carePlanRepository.GetCarePlanInfoAsync(
            request.CarePlanId,
            cancellationToken);

        if (result == null)
        {
            return new ApiResponse<CarePlanInfoDto>
            {
                Success = false,
                Message = "Care plan not found",
                Data = null
            };
        }

        return new ApiResponse<CarePlanInfoDto>
        {
            Success = true,
            Message = "Get care plan successfully",
            Data = result
        };
    }
}