using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class sla_config
{
    [Key]
    public long id { get; private set; }

    public int sla_window_hrs { get; private set; }

    public long severity_id { get; private set; }

    [ForeignKey("severity_id")]
    [InverseProperty("sla_configs")]
    public virtual incident_severity severity { get; private set; } = null!;
}
