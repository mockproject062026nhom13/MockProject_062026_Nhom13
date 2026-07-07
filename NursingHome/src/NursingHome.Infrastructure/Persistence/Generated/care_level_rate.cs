using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("care_level_id", "facility_id", "effective_from", Name = "idx_care_level_rates_lookup")]
public partial class care_level_rate
{
    [Key]
    public long id { get; private set; }

    public long care_level_id { get; private set; }

    public Guid facility_id { get; private set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal daily_rate { get; private set; }

    public DateOnly effective_from { get; private set; }

    public DateOnly? effective_to { get; private set; }

    [ForeignKey("care_level_id")]
    [InverseProperty("care_level_rates")]
    public virtual care_level care_level { get; private set; } = null!;

    [ForeignKey("facility_id")]
    [InverseProperty("care_level_rates")]
    public virtual facility facility { get; private set; } = null!;
}
