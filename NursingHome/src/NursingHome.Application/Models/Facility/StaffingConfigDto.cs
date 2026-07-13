namespace NursingHome.Application.Models.Facility;

public class StaffingConfigDto
{
    public long Id { get; set; }
    public long FacilityId { get; set; }
    public decimal MinHrsPerResidentDay { get; set; }
    public int WarnBelowPercentage { get; set; }

    // Mock data fields for Shift Breakdown without changing DB schema
    public string State { get; set; } = string.Empty;
    public string RegulationCode { get; set; } = string.Empty;
    public decimal DayCnaHours { get; set; }
    public decimal DayNurseHours { get; set; }
    public decimal EveningCnaHours { get; set; }
    public decimal EveningNurseHours { get; set; }
    public decimal NightCnaHours { get; set; }
    public decimal NightNurseHours { get; set; }
    public string EffectiveDate { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;
}
