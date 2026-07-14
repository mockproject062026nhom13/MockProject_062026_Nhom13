namespace NursingHome.Application.Features.Facilities.DTOs.Facility;

public class StaffingComplianceDto
{
    public int Census { get; set; }
    public decimal MinRequired { get; set; }
    public decimal RequiredHours { get; set; }
    public decimal ScheduledHours { get; set; }
    public decimal ActualHoursPerResident { get; set; }
    public bool IsCompliant { get; set; }
    public string Status { get; set; } = null!;
}
