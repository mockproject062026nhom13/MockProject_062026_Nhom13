using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("resident_id", Name = "idx_clinical_records_resident_id")]
public partial class clinical_record
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(50)]
    [Unicode(false)]
    public string record_type { get; private set; } = null!;

    public string description { get; private set; } = null!;

    public Guid resident_id { get; private set; }

    public Guid recorded_by { get; private set; }

    public bool is_deleted { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset updated_at { get; private set; }

    [ForeignKey("recorded_by")]
    [InverseProperty("clinical_records")]
    public virtual user recorded_byNavigation { get; private set; } = null!;

    [ForeignKey("resident_id")]
    [InverseProperty("clinical_records")]
    public virtual resident resident { get; private set; } = null!;
}
