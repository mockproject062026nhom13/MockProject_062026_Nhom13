namespace NursingHome.Application.Features.Facilities.DTOs.Facility;

public sealed class FacilityResidentStatisticDto
{
    public long FacilityId { get; set; }

    public List<LevelOfCareStatisticDto> Levels { get; set; } = [];
}

public sealed class LevelOfCareStatisticDto
{
    public long? LevelOfCareId { get; set; }

    public int Total { get; set; }
}