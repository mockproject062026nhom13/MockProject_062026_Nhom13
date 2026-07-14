using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("incidents")]
[Index("ResidentId", Name = "idx_incidents_resident_id")]
[Index("Status", Name = "idx_incidents_status")]
public partial class Incident
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("incident_type")]
    [StringLength(50)]
    [Unicode(false)]
    public string IncidentType { get; private set; } = null!;

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("description")]
    public string? Description { get; private set; }

    [Column("sla_deadline")]
    [Precision(0)]
    public DateTimeOffset SlaDeadline { get; private set; }

    [Column("resident_id")]
    public long ResidentId { get; private set; }

    [Column("severity_id")]
    public long SeverityId { get; private set; }

    [Column("reported_by")]
    public long ReportedBy { get; private set; }

    [Column("reported_at")]
    [Precision(0)]
    public DateTimeOffset ReportedAt { get; private set; }

    [InverseProperty("Incident")]
    public virtual ICollection<IncidentTimeline> IncidentTimelines { get; private set; } = new List<IncidentTimeline>();

    [ForeignKey("ReportedBy")]
    [InverseProperty("Incidents")]
    public virtual User ReportedByNavigation { get; private set; } = null!;

    [ForeignKey("ResidentId")]
    [InverseProperty("Incidents")]
    public virtual Resident Resident { get; private set; } = null!;

    [ForeignKey("SeverityId")]
    [InverseProperty("Incidents")]
    public virtual IncidentSeverity Severity { get; private set; } = null!;
}
