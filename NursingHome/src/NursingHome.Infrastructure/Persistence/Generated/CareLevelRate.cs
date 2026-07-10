using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("care_level_rates")]
[Index("CareLevelId", "FacilityId", "EffectiveFrom", Name = "idx_care_level_rates_lookup")]
public partial class CareLevelRate
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("care_level_id")]
    public long CareLevelId { get; private set; }

    [Column("facility_id")]
    public long FacilityId { get; private set; }

    [Column("daily_rate", TypeName = "decimal(18, 2)")]
    public decimal DailyRate { get; private set; }

    [Column("effective_from")]
    public DateOnly EffectiveFrom { get; private set; }

    [Column("effective_to")]
    public DateOnly? EffectiveTo { get; private set; }

    [ForeignKey("CareLevelId")]
    [InverseProperty("CareLevelRates")]
    public virtual CareLevel CareLevel { get; private set; } = null!;

    [ForeignKey("FacilityId")]
    [InverseProperty("CareLevelRates")]
    public virtual Facility Facility { get; private set; } = null!;
}
