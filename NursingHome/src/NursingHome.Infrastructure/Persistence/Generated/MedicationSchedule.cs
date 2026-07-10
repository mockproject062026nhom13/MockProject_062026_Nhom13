using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("medication_schedules")]
[Index("OrderId", Name = "idx_medication_schedules_order_id")]
public partial class MedicationSchedule
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("order_id")]
    public long OrderId { get; private set; }

    [Column("scheduled_time")]
    public TimeOnly ScheduledTime { get; private set; }

    [Column("is_active")]
    public bool IsActive { get; private set; }

    [ForeignKey("OrderId")]
    [InverseProperty("MedicationSchedules")]
    public virtual MedicationOrder Order { get; private set; } = null!;
}
