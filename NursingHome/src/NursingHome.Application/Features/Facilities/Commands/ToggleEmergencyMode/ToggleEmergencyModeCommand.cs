using MediatR;
using NursingHome.Application.Features.Facilities.DTOs.Facility;
using System.Text.Json.Serialization;

namespace NursingHome.Application.Features.Facilities.Commands.ToggleEmergencyMode;

public class ToggleEmergencyModeCommand : IRequest<StaffingRuleConfigDto>
{
    [JsonIgnore]
    public long FacilityId { get; set; }

    [JsonIgnore]
    public long PerformedByUserId { get; set; }
}
