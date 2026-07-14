using MediatR;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Features.Facilities.Queries.GetStaffingCompliance;

public record GetStaffingComplianceQuery(long FacilityId, DateOnly? Date) : IRequest<StaffingComplianceDto>;
