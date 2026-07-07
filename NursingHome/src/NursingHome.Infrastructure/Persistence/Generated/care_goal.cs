using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("care_plan_id", Name = "idx_care_goals_care_plan_id")]
public partial class care_goal
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string status { get; private set; } = null!;

    public Guid care_plan_id { get; private set; }

    [ForeignKey("care_plan_id")]
    [InverseProperty("care_goals")]
    public virtual care_plan care_plan { get; private set; } = null!;
}
