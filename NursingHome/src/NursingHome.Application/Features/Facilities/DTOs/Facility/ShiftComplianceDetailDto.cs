namespace NursingHome.Application.Features.Facilities.DTOs.Facility;

public class ShiftComplianceDetailDto
{
    public long ShiftId { get; set; }
    public string ShiftName { get; set; } = null!;
    public string ShiftTime { get; set; } = null!;
    public int RequiredNurses { get; set; }
    public int RequiredCnas { get; set; }
    public int ScheduledNurses { get; set; }
    public int ScheduledCnas { get; set; }
    public int MissingNurses { get; set; }
    public int MissingCnas { get; set; }
    public decimal CompliancePercentage { get; set; }
    public string Status { get; set; } = null!; // "Unscheduled", "Critical", "Warning", "Compliant"
    public string AlertTooltip { get; set; } = null!;
}
