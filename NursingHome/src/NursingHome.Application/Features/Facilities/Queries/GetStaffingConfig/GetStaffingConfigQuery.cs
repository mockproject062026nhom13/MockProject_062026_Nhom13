using MediatR;
using NursingHome.Application.Models.Facility;

namespace NursingHome.Application.Features.Facilities.Queries.GetStaffingConfig;

public record GetStaffingConfigQuery(long FacilityId, DateOnly? Date) : IRequest<StaffingConfigDto>;
