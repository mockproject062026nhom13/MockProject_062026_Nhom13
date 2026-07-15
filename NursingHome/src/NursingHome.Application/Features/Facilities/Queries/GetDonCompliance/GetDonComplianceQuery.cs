using MediatR;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Features.Facilities.Queries.GetDonCompliance;

public record GetDonComplianceQuery(long FacilityId, DateOnly? Date) : IRequest<ComplianceResultDto>;
