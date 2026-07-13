using MediatR;
using NursingHome.Application.Models.Facility;
using System.Text.Json.Serialization;

namespace NursingHome.Application.Features.Facilities.Commands.UpdateStaffingConfig;

public class UpdateStaffingConfigCommand : IRequest<StaffingConfigDto>
{
    [JsonIgnore]
    public long FacilityId { get; set; }
    
    public decimal MinHrsPerResidentDay { get; set; }
    public int WarnBelowPercentage { get; set; }
}
