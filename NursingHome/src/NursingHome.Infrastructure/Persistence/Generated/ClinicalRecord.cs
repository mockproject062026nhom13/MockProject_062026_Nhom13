using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("clinical_records")]
[Index("ResidentId", Name = "idx_clinical_records_resident_id")]
public partial class ClinicalRecord
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("record_type")]
    [StringLength(50)]
    [Unicode(false)]
    public string RecordType { get; private set; } = null!;

    [Column("description")]
    public string Description { get; private set; } = null!;

    [Column("resident_id")]
    public long ResidentId { get; private set; }

    [Column("recorded_by")]
    public long RecordedBy { get; private set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [Column("updated_at")]
    [Precision(0)]
    public DateTimeOffset UpdatedAt { get; private set; }

    [ForeignKey("RecordedBy")]
    [InverseProperty("ClinicalRecords")]
    public virtual User RecordedByNavigation { get; private set; } = null!;

    [ForeignKey("ResidentId")]
    [InverseProperty("ClinicalRecords")]
    public virtual Resident Resident { get; private set; } = null!;
}
