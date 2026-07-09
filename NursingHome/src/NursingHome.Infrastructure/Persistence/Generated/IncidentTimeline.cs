using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("incident_timelines")]
[Index("IncidentId", Name = "idx_incident_timelines_incident_id")]
public partial class IncidentTimeline
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("incident_id")]
    public long IncidentId { get; private set; }

    [Column("action")]
    [Unicode(false)]
    public string? Action { get; private set; }

    [Column("reason")]
    [Unicode(false)]
    public string? Reason { get; private set; }

    [Column("actor")]
    public long? Actor { get; private set; }

    [Column("createdAt")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [ForeignKey("Actor")]
    [InverseProperty("IncidentTimelines")]
    public virtual User? ActorNavigation { get; private set; }

    [ForeignKey("IncidentId")]
    [InverseProperty("IncidentTimelines")]
    public virtual Incident Incident { get; private set; } = null!;
}
