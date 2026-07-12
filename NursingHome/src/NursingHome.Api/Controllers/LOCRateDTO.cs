namespace NursingHome.Api.LOCRateDTOs;
public record CreateLocRateRequest
(
    long CareLevelId,
    long FacilityId,
    decimal DailyRate,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo
);

public record GetLocRatesRequest
(
    DateOnly FromDate,
    DateOnly ToDate
);

public record LocRateResponse
(
    long FacilityId,
    long CareLevelId,
    decimal LocRate,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo,
    bool IsPrimary
);
public record UpdateLocRateRequest
(
    long Id,

    long CareLevelId,

    long FacilityId,

    decimal DailyRate,

    DateOnly EffectiveFrom,

    DateOnly? EffectiveTo
);