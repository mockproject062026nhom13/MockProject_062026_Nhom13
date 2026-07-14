using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("vital_signs")]
[Index("ResidentId", "RecordedAt", Name = "idx_vital_signs_resident_id_recorded_at")]
public partial class VitalSign
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("resident_id")]
    public long ResidentId { get; private set; }

    [Column("recorded_by")]
    public long RecordedBy { get; private set; }

    [Column("blood_pressure_systolic")]
    public short? BloodPressureSystolic { get; private set; }

    [Column("blood_pressure_diastolic")]
    public short? BloodPressureDiastolic { get; private set; }

    [Column("heart_rate_bpm")]
    public short? HeartRateBpm { get; private set; }

    [Column("respiratory_rate")]
    public short? RespiratoryRate { get; private set; }

    [Column("temperature_fahrenheit", TypeName = "decimal(4, 1)")]
    public decimal? TemperatureFahrenheit { get; private set; }

    [Column("spo2_percentage")]
    public byte? Spo2Percentage { get; private set; }

    [Column("pain_scale")]
    public byte? PainScale { get; private set; }

    [Column("notes")]
    [StringLength(500)]
    public string? Notes { get; private set; }

    [Column("recorded_at")]
    [Precision(0)]
    public DateTimeOffset RecordedAt { get; private set; }

    [ForeignKey("RecordedBy")]
    [InverseProperty("VitalSigns")]
    public virtual User RecordedByNavigation { get; private set; } = null!;

    [ForeignKey("ResidentId")]
    [InverseProperty("VitalSigns")]
    public virtual Resident Resident { get; private set; } = null!;
}
