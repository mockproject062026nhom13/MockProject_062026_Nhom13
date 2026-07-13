using MediatR;
using NursingHome.Application.Abstractions.Repositories;
using NursingHome.Application.Models.Facility;

namespace NursingHome.Application.Features.Facilities.Commands.UpdateStaffingConfig;

public class UpdateStaffingConfigCommandHandler : IRequestHandler<UpdateStaffingConfigCommand, StaffingConfigDto>
{
    private readonly IStaffingConfigRepository _repository;

    public UpdateStaffingConfigCommandHandler(IStaffingConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task<StaffingConfigDto> Handle(UpdateStaffingConfigCommand request, CancellationToken cancellationToken)
    {
        return await _repository.AddOrUpdateAsync(
            request.FacilityId, 
            request.MinHrsPerResidentDay, 
            request.WarnBelowPercentage, 
            cancellationToken);
    }
}
