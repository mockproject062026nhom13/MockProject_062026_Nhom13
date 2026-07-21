using MediatR;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Common;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Features.Facilities.GetResidentStatisticsByFacility.Queries;

public sealed class GetResidentStatisticsByFacilityQueryHandler
    : IRequestHandler<
        GetResidentStatisticsByFacilityQuery,
        ApiResponse<List<FacilityResidentStatisticDto>>>
{
    private readonly IFacilityRepository _facilityRepository;

    public GetResidentStatisticsByFacilityQueryHandler(
        IFacilityRepository facilityRepository)
    {
        _facilityRepository = facilityRepository;
    }

    public async Task<ApiResponse<List<FacilityResidentStatisticDto>>> Handle(
        GetResidentStatisticsByFacilityQuery request,
        CancellationToken cancellationToken)
    {
        var statistics = await _facilityRepository
            .GetResidentStatisticsByFacilityAsync(cancellationToken);

        return new ApiResponse<List<FacilityResidentStatisticDto>>
        {
            Success = true,
            Message = "Get resident statistics successfully",
            Data = statistics
        };
    }
}