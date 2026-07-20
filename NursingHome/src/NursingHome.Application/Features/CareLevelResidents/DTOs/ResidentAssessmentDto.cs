namespace NursingHome.Application.Features.CareLevelResidents.DTOs;

public record ResidentAssessmentDto
{
    public int ResidentId { get; init; }
    public required string Fullname { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public required string BedNumber { get; init; }
    public required string RoomNumber { get; init; }
    public required string AssessmentType { get; init; }
    public List<IncidentDto> Incidents { get; init; } = new();
}

public record IncidentDto
{
    public required string IncidentType { get; init; }
    public DateTimeOffset ReportedAt { get; init; }
    public required string LevelName { get; init; }
}
