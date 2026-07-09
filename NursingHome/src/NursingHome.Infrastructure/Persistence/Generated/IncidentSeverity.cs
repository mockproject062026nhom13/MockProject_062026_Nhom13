using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("incident_severities")]
public partial class IncidentSeverity
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("level_name")]
    [StringLength(50)]
    public string LevelName { get; private set; } = null!;

    [Column("chart_lock_trigger")]
    public bool ChartLockTrigger { get; private set; }

    [InverseProperty("Severity")]
    public virtual ICollection<Incident> Incidents { get; private set; } = new List<Incident>();

    [InverseProperty("Severity")]
    public virtual ICollection<SlaConfig> SlaConfigs { get; private set; } = new List<SlaConfig>();
}
