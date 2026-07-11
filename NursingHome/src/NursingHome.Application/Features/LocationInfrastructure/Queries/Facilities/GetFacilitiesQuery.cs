using NursingHome.Application.Features.LocationInfrastructure;
using MediatR;

public record GetFacilitiesQuery : IRequest<List<FacilityResponse>>;
