namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class VitalSign
{
    public static VitalSign Create(
        long residentId,
        long recordedBy,
        short? bloodPressureSystolic,
        short? bloodPressureDiastolic,
        short? heartRateBpm,
        short? respiratoryRate,
        decimal? temperatureFahrenheit,
        byte? spo2Percentage,
        byte? painScale,
        string? notes,
        DateTimeOffset recordedAt)
    {
        return new VitalSign
        {
            ResidentId = residentId,
            RecordedBy = recordedBy,
            BloodPressureSystolic = bloodPressureSystolic,
            BloodPressureDiastolic = bloodPressureDiastolic,
            HeartRateBpm = heartRateBpm,
            RespiratoryRate = respiratoryRate,
            TemperatureFahrenheit = temperatureFahrenheit,
            Spo2Percentage = spo2Percentage,
            PainScale = painScale,
            Notes = notes,
            RecordedAt = recordedAt
        };
    }
    // not need yet

    // public void Update(
    //     short? bloodPressureSystolic,
    //     short? bloodPressureDiastolic,
    //     short? heartRateBpm,
    //     short? respiratoryRate,
    //     decimal? temperatureFahrenheit,
    //     byte? spo2Percentage,
    //     byte? painScale,
    //     string? notes,
    //     DateTimeOffset recordedAt)
    // {
    //     BloodPressureSystolic = bloodPressureSystolic;
    //     BloodPressureDiastolic = bloodPressureDiastolic;
    //     HeartRateBpm = heartRateBpm;
    //     RespiratoryRate = respiratoryRate;
    //     TemperatureFahrenheit = temperatureFahrenheit;
    //     Spo2Percentage = spo2Percentage;
    //     PainScale = painScale;
    //     Notes = notes;
    //     RecordedAt = recordedAt;
    // }
}