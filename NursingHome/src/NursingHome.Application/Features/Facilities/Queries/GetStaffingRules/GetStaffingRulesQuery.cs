using MediatR;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Features.Facilities.Queries.GetStaffingRules;

public record GetStaffingRulesQuery(long FacilityId) : IRequest<StaffingRuleConfigDto>;
