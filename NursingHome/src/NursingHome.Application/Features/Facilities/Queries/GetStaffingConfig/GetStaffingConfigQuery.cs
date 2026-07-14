using MediatR;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Features.Facilities.Queries.GetStaffingConfig;

public record GetStaffingConfigQuery(long FacilityId, DateOnly? Date) : IRequest<StaffingConfigDto>;
