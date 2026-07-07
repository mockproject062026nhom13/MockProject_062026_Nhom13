using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class assessment_metric
{
    [Key]
    public long id { get; private set; }

    [StringLength(50)]
    [Unicode(false)]
    public string category { get; private set; } = null!;

    [StringLength(100)]
    public string metric_name { get; private set; } = null!;

    [InverseProperty("metric")]
    public virtual ICollection<assessment_detail> assessment_details { get; private set; } = new List<assessment_detail>();
}
