using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("assessments")]
[Index("ResidentId", Name = "idx_assessments_resident_id")]
public partial class Assessment
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("adl_total_score")]
    public int AdlTotalScore { get; private set; }

    [Column("is_overridden")]
    public bool IsOverridden { get; private set; }

    [Column("suggested_care_level_id")]
    public long SuggestedCareLevelId { get; private set; }

    [Column("confirmed_care_level_id")]
    public long ConfirmedCareLevelId { get; private set; }

    [Column("resident_id")]
    public long ResidentId { get; private set; }

    [Column("assessed_by")]
    public long AssessedBy { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [ForeignKey("AssessedBy")]
    [InverseProperty("Assessments")]
    public virtual User AssessedByNavigation { get; private set; } = null!;

    [InverseProperty("Assessment")]
    public virtual ICollection<AssessmentDetail> AssessmentDetails { get; private set; } = new List<AssessmentDetail>();

    [ForeignKey("ConfirmedCareLevelId")]
    [InverseProperty("AssessmentConfirmedCareLevels")]
    public virtual CareLevel ConfirmedCareLevel { get; private set; } = null!;

    [ForeignKey("ResidentId")]
    [InverseProperty("Assessments")]
    public virtual Resident Resident { get; private set; } = null!;

    [ForeignKey("SuggestedCareLevelId")]
    [InverseProperty("AssessmentSuggestedCareLevels")]
    public virtual CareLevel SuggestedCareLevel { get; private set; } = null!;
}
