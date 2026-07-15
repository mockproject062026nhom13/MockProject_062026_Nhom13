namespace NursingHome.Application.Features.LocationInfrastructure.DTOs;

public class GetLOCRateDto
{
    public long Id { get; set; }

    public long FacilityId { get; set; }

    public long CareLevelId { get; set; }

    public decimal DailyRate { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }
}
public class LOCRateDto
{
    public long FacilityId { get; set; }

    public long CareLevelId { get; set; }

    public decimal DailyRate { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }
}

public class LocRateResponse
{
    public long FacilityId { get; set; }

    public long CareLevelId { get; set; }

    public decimal LocRate { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public bool IsPrimary { get; set; }
}

public class CreateLOCRateResponse
{
    public long FacilityId { get; set; }

    public long CareLevelId { get; set; }

    public decimal DailyRate { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }
}

public class UpdateLOCRateResponse
{
    public long Id { get; set; }

    public long FacilityId { get; set; }

    public long CareLevelId { get; set; }

    public decimal DailyRate { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }
}