using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("incident_id", Name = "idx_chart_lock_events_incident_id")]
public partial class chart_lock_event
{
    [Key]
    public Guid id { get; private set; }

    public bool locked_by_system { get; private set; }

    public string? unlock_reason { get; private set; }

    public Guid incident_id { get; private set; }

    public Guid? unlocked_by { get; private set; }

    [Precision(0)]
    public DateTimeOffset event_time { get; private set; }

    [ForeignKey("incident_id")]
    [InverseProperty("chart_lock_events")]
    public virtual incident incident { get; private set; } = null!;

    [ForeignKey("unlocked_by")]
    [InverseProperty("chart_lock_events")]
    public virtual user? unlocked_byNavigation { get; private set; }
}
