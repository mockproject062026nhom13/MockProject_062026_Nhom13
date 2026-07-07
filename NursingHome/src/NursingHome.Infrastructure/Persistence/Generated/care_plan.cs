using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("resident_id", Name = "idx_care_plans_resident_id")]
public partial class care_plan
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string status { get; private set; } = null!;

    public bool significant_change_flag { get; private set; }

    public Guid resident_id { get; private set; }

    public bool is_deleted { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset updated_at { get; private set; }

    [InverseProperty("care_plan")]
    public virtual ICollection<care_goal> care_goals { get; private set; } = new List<care_goal>();

    [InverseProperty("care_plan")]
    public virtual ICollection<care_intervention> care_interventions { get; private set; } = new List<care_intervention>();

    [ForeignKey("resident_id")]
    [InverseProperty("care_plans")]
    public virtual resident resident { get; private set; } = null!;
}
