using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("care_tasks")]
[Index("AssignedCnaId", Name = "idx_care_tasks_assigned_cna_id")]
[Index("CareInterventionId", Name = "idx_care_tasks_intervention_id")]
[Index("ScheduledTime", Name = "idx_care_tasks_scheduled_time")]
public partial class CareTask
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("task_type")]
    [StringLength(50)]
    [Unicode(false)]
    public string TaskType { get; private set; } = null!;

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("is_abnormal_flagged")]
    public bool IsAbnormalFlagged { get; private set; }

    [Column("care_intervention_id")]
    public long CareInterventionId { get; private set; }

    [Column("assigned_cna_id")]
    public long? AssignedCnaId { get; private set; }

    [Column("scheduled_time")]
    [Precision(0)]
    public DateTimeOffset ScheduledTime { get; private set; }

    [Column("completed_at")]
    [Precision(0)]
    public DateTimeOffset? CompletedAt { get; private set; }

    [ForeignKey("AssignedCnaId")]
    [InverseProperty("CareTasks")]
    public virtual User? AssignedCna { get; private set; }

    [ForeignKey("CareInterventionId")]
    [InverseProperty("CareTasks")]
    public virtual CareIntervention CareIntervention { get; private set; } = null!;
}
