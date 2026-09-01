namespace NursingHome.Application.Features.Facilities.DTOs.Facility;

public class ComplianceResultDto
{
    public int Census { get; set; }
    public DateOnly Date { get; set; }
    public bool IsEmergencyModeActive { get; set; }
    public List<ShiftComplianceDetailDto> ShiftDetails { get; set; } = new();
}
