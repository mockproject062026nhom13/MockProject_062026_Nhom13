using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("shifts")]
[Index("FacilityId", Name = "idx_shifts_facility_id")]
public partial class Shift
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("facility_id")]
    public long FacilityId { get; private set; }

    [Column("shift_name")]
    [StringLength(20)]
    [Unicode(false)]
    public string ShiftName { get; private set; } = null!;

    [Column("start_time")]
    public TimeOnly StartTime { get; private set; }

    [Column("end_time")]
    public TimeOnly EndTime { get; private set; }

    [Column("description")]
    [StringLength(255)]
    public string? Description { get; private set; }

    [ForeignKey("FacilityId")]
    [InverseProperty("Shifts")]
    public virtual Facility Facility { get; private set; } = null!;

    [InverseProperty("Shift")]
    public virtual ICollection<ShiftAssignment> ShiftAssignments { get; private set; } = new List<ShiftAssignment>();
}
