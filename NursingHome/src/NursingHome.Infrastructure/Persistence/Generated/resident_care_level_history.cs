using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("resident_care_level_history")]
[Index("resident_id", Name = "idx_resident_care_level_history_resident_id")]
public partial class resident_care_level_history
{
    [Key]
    public Guid id { get; private set; }

    public DateOnly start_date { get; private set; }

    public DateOnly? end_date { get; private set; }

    public Guid resident_id { get; private set; }

    public long care_level_id { get; private set; }

    [ForeignKey("care_level_id")]
    [InverseProperty("resident_care_level_histories")]
    public virtual care_level care_level { get; private set; } = null!;

    [ForeignKey("resident_id")]
    [InverseProperty("resident_care_level_histories")]
    public virtual resident resident { get; private set; } = null!;
}
