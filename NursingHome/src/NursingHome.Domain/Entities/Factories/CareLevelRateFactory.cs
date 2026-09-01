namespace NursingHome.Domain.Factories;

using NursingHome.Domain.Entities;

public static class CareLevelRateFactory
{
    public static CareLevelRate Hydrate(
        long id,
        long careLevelId,
        long facilityId,
        decimal dailyRate,
        DateOnly effectiveFrom,
        DateOnly? effectiveTo)
    {
        return new CareLevelRate(
            id,
            careLevelId,
            facilityId,
            dailyRate,
            effectiveFrom,
            effectiveTo);
    }
}