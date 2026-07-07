using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("resident_id", Name = "idx_admissions_resident_id")]
public partial class admission
{
    [Key]
    public Guid id { get; private set; }

    public DateOnly admission_date { get; private set; }

    public DateOnly? discharge_date { get; private set; }

    [StringLength(255)]
    public string? discharge_reason { get; private set; }

    public Guid resident_id { get; private set; }

    public Guid facility_id { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [ForeignKey("facility_id")]
    [InverseProperty("admissions")]
    public virtual facility facility { get; private set; } = null!;

    [ForeignKey("resident_id")]
    [InverseProperty("admissions")]
    public virtual resident resident { get; private set; } = null!;
}
