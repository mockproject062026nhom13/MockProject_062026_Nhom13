using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("care_levels")]
[Index("LevelCode", Name = "UQ__care_lev__9AEFB2943EC8BC75", IsUnique = true)]
public partial class CareLevel
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("level_code")]
    [StringLength(30)]
    [Unicode(false)]
    public string LevelCode { get; private set; } = null!;

    [Column("level_name")]
    [StringLength(100)]
    public string LevelName { get; private set; } = null!;

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [InverseProperty("ConfirmedCareLevel")]
    public virtual ICollection<Assessment> AssessmentConfirmedCareLevels { get; private set; } = new List<Assessment>();

    [InverseProperty("SuggestedCareLevel")]
    public virtual ICollection<Assessment> AssessmentSuggestedCareLevels { get; private set; } = new List<Assessment>();

    [InverseProperty("CareLevel")]
    public virtual ICollection<CareLevelRate> CareLevelRates { get; private set; } = new List<CareLevelRate>();

    [InverseProperty("CareLevel")]
    public virtual ICollection<ResidentCareLevelHistory> ResidentCareLevelHistories { get; private set; } = new List<ResidentCareLevelHistory>();
}
