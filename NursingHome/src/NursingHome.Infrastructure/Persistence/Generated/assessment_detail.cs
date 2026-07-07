using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("assessment_id", Name = "idx_assessment_details_assessment_id")]
public partial class assessment_detail
{
    [Key]
    public Guid id { get; private set; }

    public int score { get; private set; }

    [StringLength(500)]
    public string? notes { get; private set; }

    public Guid assessment_id { get; private set; }

    public long metric_id { get; private set; }

    [ForeignKey("assessment_id")]
    [InverseProperty("assessment_details")]
    public virtual assessment assessment { get; private set; } = null!;

    [ForeignKey("metric_id")]
    [InverseProperty("assessment_details")]
    public virtual assessment_metric metric { get; private set; } = null!;
}
