using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("shift_assignments")]
[Index("UserId", "WorkDate", Name = "idx_shift_assignments_user_id_work_date")]
[Index("ShiftId", "UserId", "WorkDate", Name = "uq_shift_assignment", IsUnique = true)]
public partial class ShiftAssignment
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("shift_id")]
    public long ShiftId { get; private set; }

    [Column("user_id")]
    public long UserId { get; private set; }

    [Column("work_date")]
    public DateOnly WorkDate { get; private set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("clock_in_at")]
    [Precision(0)]
    public DateTimeOffset? ClockInAt { get; private set; }

    [Column("clock_out_at")]
    [Precision(0)]
    public DateTimeOffset? ClockOutAt { get; private set; }

    [ForeignKey("ShiftId")]
    [InverseProperty("ShiftAssignments")]
    public virtual Shift Shift { get; private set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("ShiftAssignments")]
    public virtual User User { get; private set; } = null!;
}
