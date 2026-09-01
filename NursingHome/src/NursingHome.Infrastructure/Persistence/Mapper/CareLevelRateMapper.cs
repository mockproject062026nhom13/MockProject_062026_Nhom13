using NursingHome.Domain.Factories;

namespace NursingHome.Infrastructure.Persistence.Mappers;

public static class CareLevelRateMapper
{
    public static Domain.Entities.CareLevelRate ToDomain(
        this Generated.CareLevelRate entity)
    {
        return CareLevelRateFactory.Hydrate(
            entity.Id,
            entity.CareLevelId,
            entity.FacilityId,
            entity.DailyRate,
            entity.EffectiveFrom,
            entity.EffectiveTo);
    }

    public static void UpdatePersistence(
        this Generated.CareLevelRate entity,
        Domain.Entities.CareLevelRate domain)
    {
        entity.Update(
            domain.CareLevelId,
            domain.FacilityId,
            domain.DailyRate,
            domain.EffectiveFrom,
            domain.EffectiveTo);
    }
}