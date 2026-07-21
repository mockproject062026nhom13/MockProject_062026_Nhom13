using NursingHome.Application.Features.LocationInfrastructure;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Abstractions;

public interface IFacilityRepository
{
    Task<List<FacilityResponse>> GetFacilitiesAsync(CancellationToken ct);

    Task<List<FacilityResidentStatisticDto>> GetResidentStatisticsByFacilityAsync(
        CancellationToken cancellationToken);

}
