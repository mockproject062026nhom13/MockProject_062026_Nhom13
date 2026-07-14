namespace NursingHome.Application.Features.Facilities.DTOs.Facility;

public class StaffingConfigDto
{
    public long Id { get; set; }
    public long FacilityId { get; set; }
    public decimal MinHrsPerResidentDay { get; set; }
    public int WarnBelowPercentage { get; set; }

    // Shift Breakdown
    public decimal DayCnaHours { get; set; }
    public decimal DayNurseHours { get; set; }
    public decimal EveningCnaHours { get; set; }
    public decimal EveningNurseHours { get; set; }
    public decimal NightCnaHours { get; set; }
    public decimal NightNurseHours { get; set; }
}
