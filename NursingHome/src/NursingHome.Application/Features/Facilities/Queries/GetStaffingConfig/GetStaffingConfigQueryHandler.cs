using MediatR;
using NursingHome.Application.Abstractions.Repositories;
using NursingHome.Application.Models.Facility;
using NursingHome.Domain.Exceptions;

namespace NursingHome.Application.Features.Facilities.Queries.GetStaffingConfig;

public class GetStaffingConfigQueryHandler : IRequestHandler<GetStaffingConfigQuery, StaffingConfigDto>
{
    private readonly IStaffingConfigRepository _repository;

    public GetStaffingConfigQueryHandler(IStaffingConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task<StaffingConfigDto> Handle(GetStaffingConfigQuery request, CancellationToken cancellationToken)
    {
        var config = await _repository.GetByFacilityIdAsync(request.FacilityId, cancellationToken);
        
        if (config == null)
        {
            throw new NotFoundException($"Staffing config for FacilityId {request.FacilityId} not found.");
        }

        return config;
    }
}
