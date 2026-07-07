using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("resident_id", "recorded_at", Name = "idx_vital_signs_resident_id_recorded_at", IsDescending = new[] { false, true })]
public partial class vital_sign
{
    [Key]
    public long id { get; private set; }

    public Guid resident_id { get; private set; }

    public Guid recorded_by { get; private set; }

    public short? blood_pressure_systolic { get; private set; }

    public short? blood_pressure_diastolic { get; private set; }

    public short? heart_rate_bpm { get; private set; }

    public short? respiratory_rate { get; private set; }

    [Column(TypeName = "decimal(4, 1)")]
    public decimal? temperature_fahrenheit { get; private set; }

    public byte? spo2_percentage { get; private set; }

    public byte? pain_scale { get; private set; }

    [StringLength(500)]
    public string? notes { get; private set; }

    [Precision(0)]
    public DateTimeOffset recorded_at { get; private set; }

    [ForeignKey("recorded_by")]
    [InverseProperty("vital_signs")]
    public virtual user recorded_byNavigation { get; private set; } = null!;

    [ForeignKey("resident_id")]
    [InverseProperty("vital_signs")]
    public virtual resident resident { get; private set; } = null!;
}
