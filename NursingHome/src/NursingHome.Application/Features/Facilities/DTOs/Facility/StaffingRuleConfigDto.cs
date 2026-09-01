namespace NursingHome.Application.Features.Facilities.DTOs.Facility;

public class StaffingRuleConfigDto
{
    public bool IsEmergencyMode { get; set; }
    public Dictionary<string, ShiftRatiosDto> Standard { get; set; } = new();
    public Dictionary<string, ShiftRatiosDto> Emergency { get; set; } = new();
}

public class ShiftRatiosDto
{
    public RatioDetailDto Day { get; set; } = new();
    public RatioDetailDto Evening { get; set; } = new();
    public RatioDetailDto Night { get; set; } = new();
}

public class RatioDetailDto
{
    public int Nurse { get; set; }
    public int Cna { get; set; }
}
