using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("resident_id", Name = "idx_assessments_resident_id")]
public partial class assessment
{
    [Key]
    public Guid id { get; private set; }

    public int adl_total_score { get; private set; }

    public bool is_overridden { get; private set; }

    public long suggested_care_level_id { get; private set; }

    public long confirmed_care_level_id { get; private set; }

    public Guid resident_id { get; private set; }

    public Guid assessed_by { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [ForeignKey("assessed_by")]
    [InverseProperty("assessments")]
    public virtual user assessed_byNavigation { get; private set; } = null!;

    [InverseProperty("assessment")]
    public virtual ICollection<assessment_detail> assessment_details { get; private set; } = new List<assessment_detail>();

    [ForeignKey("confirmed_care_level_id")]
    [InverseProperty("assessmentconfirmed_care_levels")]
    public virtual care_level confirmed_care_level { get; private set; } = null!;

    [ForeignKey("resident_id")]
    [InverseProperty("assessments")]
    public virtual resident resident { get; private set; } = null!;

    [ForeignKey("suggested_care_level_id")]
    [InverseProperty("assessmentsuggested_care_levels")]
    public virtual care_level suggested_care_level { get; private set; } = null!;
}
