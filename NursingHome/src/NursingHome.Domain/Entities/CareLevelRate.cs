namespace NursingHome.Domain.Entities;

public class CareLevelRate
{
    public long Id { get; private set; }

    public long CareLevelId { get; private set; }

    public long FacilityId { get; private set; }

    public decimal DailyRate { get; private set; }

    public DateOnly EffectiveFrom { get; private set; }

    public DateOnly? EffectiveTo { get; private set; }

    private CareLevelRate()
    {
    }

    public CareLevelRate(
        long careLevelId,
        long facilityId,
        decimal dailyRate,
        DateOnly effectiveFrom,
        DateOnly? effectiveTo)
    {
        CareLevelId = careLevelId;
        FacilityId = facilityId;
        DailyRate = dailyRate;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
    }

    internal CareLevelRate(
        long id,
        long careLevelId,
        long facilityId,
        decimal dailyRate,
        DateOnly effectiveFrom,
        DateOnly? effectiveTo)
    {
        Id = id;
        CareLevelId = careLevelId;
        FacilityId = facilityId;
        DailyRate = dailyRate;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
    }

    public void Update(
        long careLevelId,
        long facilityId,
        decimal dailyRate,
        DateOnly effectiveFrom,
        DateOnly? effectiveTo)
    {
        CareLevelId = careLevelId;
        FacilityId = facilityId;
        DailyRate = dailyRate;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
    }
}