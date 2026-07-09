using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("care_goals")]
[Index("CarePlanId", Name = "idx_care_goals_care_plan_id")]
public partial class CareGoal
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("care_plan_id")]
    public long CarePlanId { get; private set; }

    [ForeignKey("CarePlanId")]
    [InverseProperty("CareGoals")]
    public virtual CarePlan CarePlan { get; private set; } = null!;
}
