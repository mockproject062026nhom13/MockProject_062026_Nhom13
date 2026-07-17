namespace NursingHome.Application.Features.CareLevelResidents.DTOs;

public record ResidentAssessmentDto
{
    public int ResidentId { get; init; }
    public string Fullname { get; init; }
    public DateTime CreatedAt { get; init; }
    public string BedNumber { get; init; }
    public string RoomNumber { get; init; }
    public string AssessmentType { get; init; }
    public List<IncidentDto> Incidents { get; init; } = new();
}

public record IncidentDto
{
    public string IncidentType { get; init; }
    public DateTime ReportedAt { get; init; }
    public string LevelName { get; init; }
}
