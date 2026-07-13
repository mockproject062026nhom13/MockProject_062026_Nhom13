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
        var targetDate = request.Date ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var config = await _repository.GetConfigWithRealBreakdownAsync(request.FacilityId, targetDate, cancellationToken);
        
        if (config == null)
        {
            throw new NotFoundException("StaffingConfig", request.FacilityId);
        }

        return config;
    }
}
