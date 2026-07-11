using MediatR;
using NursingHome.Application.Abstractions;

namespace NursingHome.Application.Features.LocationInfrastructure;

public class GetFacilitiesQueryHandler(IFacilityRepository repository)
    : IRequestHandler<GetFacilitiesQuery, List<FacilityResponse>>
{
    public async Task<List<FacilityResponse>> Handle(
        GetFacilitiesQuery request,
        CancellationToken ct)
    {
        return await repository.GetFacilitiesAsync(ct);
    }
}
