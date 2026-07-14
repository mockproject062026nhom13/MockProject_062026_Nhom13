using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("care_plans")]
[Index("ResidentId", Name = "idx_care_plans_resident_id")]
public partial class CarePlan
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("significant_change_flag")]
    public bool SignificantChangeFlag { get; private set; }

    [Column("resident_id")]
    public long ResidentId { get; private set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [Column("updated_at")]
    [Precision(0)]
    public DateTimeOffset UpdatedAt { get; private set; }

    [InverseProperty("CarePlan")]
    public virtual ICollection<CareGoal> CareGoals { get; private set; } = new List<CareGoal>();

    [InverseProperty("CarePlan")]
    public virtual ICollection<CareIntervention> CareInterventions { get; private set; } = new List<CareIntervention>();

    [ForeignKey("ResidentId")]
    [InverseProperty("CarePlans")]
    public virtual Resident Resident { get; private set; } = null!;
}
