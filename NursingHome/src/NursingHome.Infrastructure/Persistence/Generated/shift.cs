using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("facility_id", Name = "idx_shifts_facility_id")]
public partial class shift
{
    [Key]
    public long id { get; private set; }

    public Guid facility_id { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string shift_name { get; private set; } = null!;

    public TimeOnly start_time { get; private set; }

    public TimeOnly end_time { get; private set; }

    [ForeignKey("facility_id")]
    [InverseProperty("shifts")]
    public virtual facility facility { get; private set; } = null!;

    [InverseProperty("shift")]
    public virtual ICollection<shift_assignment> shift_assignments { get; private set; } = new List<shift_assignment>();
}
