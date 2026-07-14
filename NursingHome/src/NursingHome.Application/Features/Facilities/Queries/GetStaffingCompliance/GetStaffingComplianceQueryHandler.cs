using MediatR;
using NursingHome.Application.Abstractions.Repositories;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Features.Facilities.Queries.GetStaffingCompliance;

public class GetStaffingComplianceQueryHandler : IRequestHandler<GetStaffingComplianceQuery, StaffingComplianceDto>
{
    private readonly IStaffingComplianceRepository _repository;

    public GetStaffingComplianceQueryHandler(IStaffingComplianceRepository repository)
    {
        _repository = repository;
    }

    public async Task<StaffingComplianceDto> Handle(GetStaffingComplianceQuery request, CancellationToken cancellationToken)
    {
        var targetDate = request.Date ?? DateOnly.FromDateTime(DateTime.UtcNow);
        return await _repository.GetComplianceAsync(request.FacilityId, targetDate, cancellationToken);
    }
}
