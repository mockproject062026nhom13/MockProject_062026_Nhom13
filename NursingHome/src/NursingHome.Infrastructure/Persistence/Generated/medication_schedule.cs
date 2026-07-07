using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("order_id", Name = "idx_medication_schedules_order_id")]
public partial class medication_schedule
{
    [Key]
    public long id { get; private set; }

    public Guid order_id { get; private set; }

    public TimeOnly scheduled_time { get; private set; }

    public bool is_active { get; private set; }

    [ForeignKey("order_id")]
    [InverseProperty("medication_schedules")]
    public virtual medication_order order { get; private set; } = null!;
}
