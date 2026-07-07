using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("assigned_cna_id", Name = "idx_care_tasks_assigned_cna_id")]
[Index("care_intervention_id", Name = "idx_care_tasks_intervention_id")]
[Index("scheduled_time", Name = "idx_care_tasks_scheduled_time")]
public partial class care_task
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(50)]
    [Unicode(false)]
    public string task_type { get; private set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string status { get; private set; } = null!;

    public bool is_abnormal_flagged { get; private set; }

    public Guid care_intervention_id { get; private set; }

    public Guid? assigned_cna_id { get; private set; }

    [Precision(0)]
    public DateTimeOffset scheduled_time { get; private set; }

    [Precision(0)]
    public DateTimeOffset? completed_at { get; private set; }

    [ForeignKey("assigned_cna_id")]
    [InverseProperty("care_tasks")]
    public virtual user? assigned_cna { get; private set; }

    [ForeignKey("care_intervention_id")]
    [InverseProperty("care_tasks")]
    public virtual care_intervention care_intervention { get; private set; } = null!;
}
