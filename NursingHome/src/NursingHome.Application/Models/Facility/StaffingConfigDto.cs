namespace NursingHome.Application.Models.Facility;

public class StaffingConfigDto
{
    public long Id { get; set; }
    public long FacilityId { get; set; }
    public decimal MinHrsPerResidentDay { get; set; }
    public int WarnBelowPercentage { get; set; }
}
