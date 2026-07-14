using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("care_interventions")]
[Index("CarePlanId", Name = "idx_care_interventions_care_plan_id")]
public partial class CareIntervention
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("assigned_role")]
    [StringLength(50)]
    [Unicode(false)]
    public string AssignedRole { get; private set; } = null!;

    [Column("care_plan_id")]
    public long CarePlanId { get; private set; }

    [ForeignKey("CarePlanId")]
    [InverseProperty("CareInterventions")]
    public virtual CarePlan CarePlan { get; private set; } = null!;

    [InverseProperty("CareIntervention")]
    public virtual ICollection<CareTask> CareTasks { get; private set; } = new List<CareTask>();
}
