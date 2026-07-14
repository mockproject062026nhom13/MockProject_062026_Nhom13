using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.CareLevelResidents.DTOs;

namespace NursingHome.Application.Features.CareLevelResidents.Queries;

public class GetResidentStatisticsQueryHandler(ICareLevelResidentRepository _repository) 
    : IRequestHandler<GetResidentStatisticsQuery, ApiResponse<ResidentStatisticsDto>>
{
    public async Task<ApiResponse<ResidentStatisticsDto>> Handle(GetResidentStatisticsQuery request, CancellationToken cancellationToken)
    {
        var statistics = await _repository.GetResidentStatisticsAsync(cancellationToken);

        return ApiResponse<ResidentStatisticsDto>.CreateSuccess(
            data: statistics,
            statusCode: 200,
            message: "Resident statistics retrieved successfully."
        );
    }
}