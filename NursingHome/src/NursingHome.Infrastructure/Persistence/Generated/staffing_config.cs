using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("facility_id", Name = "idx_staffing_configs_facility_id")]
public partial class staffing_config
{
    [Key]
    public long id { get; private set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal min_hrs_per_resident_day { get; private set; }

    public int warn_below_percentage { get; private set; }

    public Guid facility_id { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [ForeignKey("facility_id")]
    [InverseProperty("staffing_configs")]
    public virtual facility facility { get; private set; } = null!;
}
