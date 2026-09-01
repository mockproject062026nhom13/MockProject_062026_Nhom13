using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("pre_admission_screenings")]
[Index("ResidentId", Name = "idx_pre_admission_screenings_resident_id")]
public partial class PreAdmissionScreening
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("resident_id")]
    public long ResidentId { get; private set; }

    [Column("screened_by")]
    public long ScreenedBy { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [ForeignKey("ResidentId")]
    [InverseProperty("PreAdmissionScreenings")]
    public virtual Resident Resident { get; private set; } = null!;

    [ForeignKey("ScreenedBy")]
    [InverseProperty("PreAdmissionScreenings")]
    public virtual User ScreenedByNavigation { get; private set; } = null!;
}
