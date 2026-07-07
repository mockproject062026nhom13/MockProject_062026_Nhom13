using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("resident_id", Name = "idx_incidents_resident_id")]
[Index("status", Name = "idx_incidents_status")]
public partial class incident
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(50)]
    [Unicode(false)]
    public string incident_type { get; private set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string status { get; private set; } = null!;

    public string? description { get; private set; }

    [Precision(0)]
    public DateTimeOffset sla_deadline { get; private set; }

    public Guid resident_id { get; private set; }

    public long severity_id { get; private set; }

    public Guid reported_by { get; private set; }

    [Precision(0)]
    public DateTimeOffset reported_at { get; private set; }

    [InverseProperty("incident")]
    public virtual ICollection<chart_lock_event> chart_lock_events { get; private set; } = new List<chart_lock_event>();

    [ForeignKey("reported_by")]
    [InverseProperty("incidents")]
    public virtual user reported_byNavigation { get; private set; } = null!;

    [ForeignKey("resident_id")]
    [InverseProperty("incidents")]
    public virtual resident resident { get; private set; } = null!;

    [ForeignKey("severity_id")]
    [InverseProperty("incidents")]
    public virtual incident_severity severity { get; private set; } = null!;
}
