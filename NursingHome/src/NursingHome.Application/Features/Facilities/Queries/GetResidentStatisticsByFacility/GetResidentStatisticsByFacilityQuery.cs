using MediatR;
using NursingHome.Application.Common;
using NursingHome.Application.Features.Facilities.DTOs.Facility;


namespace NursingHome.Application.Features.Facilities.GetResidentStatisticsByFacility.Queries;

public sealed record GetResidentStatisticsByFacilityQuery()
    : IRequest<ApiResponse<List<FacilityResidentStatisticDto>>>;