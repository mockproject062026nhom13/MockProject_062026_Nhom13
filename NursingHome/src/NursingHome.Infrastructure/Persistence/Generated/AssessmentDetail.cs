using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("assessment_details")]
[Index("AssessmentId", Name = "idx_assessment_details_assessment_id")]
public partial class AssessmentDetail
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("score")]
    public int Score { get; private set; }

    [Column("notes")]
    [StringLength(500)]
    public string? Notes { get; private set; }

    [Column("assessment_id")]
    public long AssessmentId { get; private set; }

    [Column("metric_id")]
    public long MetricId { get; private set; }

    [ForeignKey("AssessmentId")]
    [InverseProperty("AssessmentDetails")]
    public virtual Assessment Assessment { get; private set; } = null!;

    [ForeignKey("MetricId")]
    [InverseProperty("AssessmentDetails")]
    public virtual AssessmentMetric Metric { get; private set; } = null!;
}
