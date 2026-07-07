using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("user_id", "work_date", Name = "idx_shift_assignments_user_id_work_date")]
[Index("shift_id", "user_id", "work_date", Name = "uq_shift_assignment", IsUnique = true)]
public partial class shift_assignment
{
    [Key]
    public long id { get; private set; }

    public long shift_id { get; private set; }

    public Guid user_id { get; private set; }

    public DateOnly work_date { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string status { get; private set; } = null!;

    [Precision(0)]
    public DateTimeOffset? clock_in_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset? clock_out_at { get; private set; }

    [ForeignKey("shift_id")]
    [InverseProperty("shift_assignments")]
    public virtual shift shift { get; private set; } = null!;

    [ForeignKey("user_id")]
    [InverseProperty("shift_assignments")]
    public virtual user user { get; private set; } = null!;
}
