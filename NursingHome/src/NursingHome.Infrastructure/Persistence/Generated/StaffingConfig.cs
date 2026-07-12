using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("staffing_configs")]
[Index("FacilityId", Name = "idx_staffing_configs_facility_id")]
public partial class StaffingConfig
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("min_hrs_per_resident_day", TypeName = "decimal(5, 2)")]
    public decimal MinHrsPerResidentDay { get; private set; }

    [Column("warn_below_percentage")]
    public int WarnBelowPercentage { get; private set; }

    [Column("facility_id")]
    public long FacilityId { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [ForeignKey("FacilityId")]
    [InverseProperty("StaffingConfigs")]
    public virtual Facility Facility { get; private set; } = null!;
}
