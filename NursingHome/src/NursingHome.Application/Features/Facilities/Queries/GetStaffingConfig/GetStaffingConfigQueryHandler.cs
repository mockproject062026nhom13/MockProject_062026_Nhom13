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
            throw new NotFoundException("StaffingConfig", request.FacilityId);
        }

        return new StaffingConfigDto
        {
            Id = config.Id,
            FacilityId = config.FacilityId,
            MinHrsPerResidentDay = config.MinHrsPerResidentDay,
            WarnBelowPercentage = config.WarnBelowPercentage,
            
            // Auto-calculated / Simulated fields
            State = "California",
            RegulationCode = "BR-01",
            DayCnaHours = Math.Round(config.MinHrsPerResidentDay * 0.40m, 2),
            DayNurseHours = Math.Round(config.MinHrsPerResidentDay * 0.25m, 2),
            EveningCnaHours = Math.Round(config.MinHrsPerResidentDay * 0.175m, 2),
            EveningNurseHours = Math.Round(config.MinHrsPerResidentDay * 0.125m, 2),
            NightCnaHours = Math.Round(config.MinHrsPerResidentDay * 0.125m, 2),
            NightNurseHours = Math.Round(config.MinHrsPerResidentDay * 0.075m, 2),
            EffectiveDate = "2026-06-29",
            UpdatedBy = "System Admin"
        };
    }
}
