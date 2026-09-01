using MediatR;
using NursingHome.Application.Features.Facilities.DTOs.Facility;
using System.Text.Json.Serialization;

namespace NursingHome.Application.Features.Facilities.Commands.UpdateStaffingRules;

public class UpdateStaffingRulesCommand : IRequest<Unit>
{
    [JsonIgnore]
    public long FacilityId { get; set; }

    [JsonIgnore]
    public long PerformedByUserId { get; set; }

    public Dictionary<string, ShiftRatiosDto> Standard { get; set; } = new();
    public Dictionary<string, ShiftRatiosDto> Emergency { get; set; } = new();
}
