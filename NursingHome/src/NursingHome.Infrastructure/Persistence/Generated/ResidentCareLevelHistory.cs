using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("resident_care_level_history")]
[Index("ResidentId", Name = "idx_resident_care_level_history_resident_id")]
public partial class ResidentCareLevelHistory
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("start_date")]
    public DateOnly StartDate { get; private set; }

    [Column("end_date")]
    public DateOnly? EndDate { get; private set; }

    [Column("resident_id")]
    public long ResidentId { get; private set; }

    [Column("care_level_id")]
    public long CareLevelId { get; private set; }

    [ForeignKey("CareLevelId")]
    [InverseProperty("ResidentCareLevelHistories")]
    public virtual CareLevel CareLevel { get; private set; } = null!;

    [ForeignKey("ResidentId")]
    [InverseProperty("ResidentCareLevelHistories")]
    public virtual Resident Resident { get; private set; } = null!;
}
