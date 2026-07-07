using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class incident_severity
{
    [Key]
    public long id { get; private set; }

    [StringLength(50)]
    public string level_name { get; private set; } = null!;

    public bool chart_lock_trigger { get; private set; }

    [InverseProperty("severity")]
    public virtual ICollection<incident> incidents { get; private set; } = new List<incident>();

    [InverseProperty("severity")]
    public virtual ICollection<sla_config> sla_configs { get; private set; } = new List<sla_config>();
}
