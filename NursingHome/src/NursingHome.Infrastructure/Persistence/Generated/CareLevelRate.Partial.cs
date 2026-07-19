namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class CareLevelRate
{
    public static CareLevelRate Create(
        long careLevelId,
        long facilityId,
        decimal dailyRate,
        DateOnly effectiveFrom,
        DateOnly? effectiveTo)
    {
        return new CareLevelRate
        {
            CareLevelId = careLevelId,
            FacilityId = facilityId,
            DailyRate = dailyRate,
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo
        };
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