using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("sla_configs")]
public partial class SlaConfig
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("sla_window_hrs")]
    public int SlaWindowHrs { get; private set; }

    [Column("severity_id")]
    public long SeverityId { get; private set; }

    [ForeignKey("SeverityId")]
    [InverseProperty("SlaConfigs")]
    public virtual IncidentSeverity Severity { get; private set; } = null!;
}
