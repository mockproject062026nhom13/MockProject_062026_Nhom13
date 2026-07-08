using NursingHome.Application.Features.LocationInfrastructure;

public record GetFacilitiesQuery : IRequest<List<FacilityResponse>>;
