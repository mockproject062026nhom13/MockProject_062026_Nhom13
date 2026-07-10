using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("assessment_metrics")]
public partial class AssessmentMetric
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("category")]
    [StringLength(50)]
    [Unicode(false)]
    public string Category { get; private set; } = null!;

    [Column("metric_name")]
    [StringLength(100)]
    public string MetricName { get; private set; } = null!;

    [InverseProperty("Metric")]
    public virtual ICollection<AssessmentDetail> AssessmentDetails { get; private set; } = new List<AssessmentDetail>();
}
