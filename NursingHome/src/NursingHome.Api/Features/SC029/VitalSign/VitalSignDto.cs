namespace NursingHome.Api.Features.VitalSign;

public record VitalSignDto(
    string UserName,
    byte? Spo2Percentage,
    DateTimeOffset RecordedAt
);
