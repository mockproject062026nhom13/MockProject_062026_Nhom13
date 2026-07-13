using MediatR;
using NursingHome.Application.Models.Facility;

namespace NursingHome.Application.Features.Facilities.Queries.GetStaffingCompliance;

public record GetStaffingComplianceQuery(long FacilityId, DateOnly? Date) : IRequest<StaffingComplianceDto>;
