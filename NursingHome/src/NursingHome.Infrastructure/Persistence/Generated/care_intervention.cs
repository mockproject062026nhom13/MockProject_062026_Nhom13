using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("care_plan_id", Name = "idx_care_interventions_care_plan_id")]
public partial class care_intervention
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(50)]
    [Unicode(false)]
    public string assigned_role { get; private set; } = null!;

    public Guid care_plan_id { get; private set; }

    [ForeignKey("care_plan_id")]
    [InverseProperty("care_interventions")]
    public virtual care_plan care_plan { get; private set; } = null!;

    [InverseProperty("care_intervention")]
    public virtual ICollection<care_task> care_tasks { get; private set; } = new List<care_task>();
}
