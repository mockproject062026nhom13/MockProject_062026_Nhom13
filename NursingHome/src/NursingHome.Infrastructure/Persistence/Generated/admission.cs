using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("admissions")]
[Index("ResidentId", Name = "idx_admissions_resident_id")]
public partial class Admission
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("admission_date")]
    public DateOnly AdmissionDate { get; private set; }

    [Column("discharge_date")]
    public DateOnly? DischargeDate { get; private set; }

    [Column("discharge_reason")]
    [StringLength(255)]
    public string? DischargeReason { get; private set; }

    [Column("resident_id")]
    public long ResidentId { get; private set; }

    [Column("facility_id")]
    public long FacilityId { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [ForeignKey("FacilityId")]
    [InverseProperty("Admissions")]
    public virtual Facility Facility { get; private set; } = null!;

    [ForeignKey("ResidentId")]
    [InverseProperty("Admissions")]
    public virtual Resident Resident { get; private set; } = null!;
}
