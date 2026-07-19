using NursingHome.Application.Features.LocationInfrastructure;

namespace NursingHome.Application.Abstractions;

public interface IFacilityRepository
{
    Task<List<FacilityResponse>> GetFacilitiesAsync(CancellationToken ct);
}
