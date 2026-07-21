namespace NursingHome.Application.Features.CareLevelResidents.DTOs;

public class AssessmentCreateDTO
{
    // in db, dosent have a field for Allergy so we skipped it in the DTO, but we can add it later if needed
    public long ResidentId { get; set; }

    public long AssessedBy { get; set; }

    public List<AssessmentDetailCreateDTO> AssessmentDetails { get; set; } = [];

    public VitalSignCreateDTO VitalSigns { get; set; } = new();
}



public class AssessmentDetailCreateDTO
{
    public long MetricId { get; set; }

    public int Score { get; set; }

    public string? Notes { get; set; }
}


public class VitalSignCreateDTO
{
    public short? BloodPressureSystolic { get; set; }

    public short? BloodPressureDiastolic { get; set; }

    public short? HeartRateBpm { get; set; }

    public short? RespiratoryRate { get; set; }

    public decimal? TemperatureFahrenheit { get; set; }

    public byte? Spo2Percentage { get; set; }

    public byte? PainScale { get; set; }

    public string? Notes { get; set; }
}


public class CreateAssessmentResponse
{
    public long AssessmentId { get; set; }

    public long ResidentId { get; set; }

    public int AdlTotalScore { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}