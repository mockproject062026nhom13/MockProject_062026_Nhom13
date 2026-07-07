using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("level_code", Name = "UQ__care_lev__9AEFB29473C4B677", IsUnique = true)]
public partial class care_level
{
    [Key]
    public long id { get; private set; }

    [StringLength(30)]
    [Unicode(false)]
    public string level_code { get; private set; } = null!;

    [StringLength(100)]
    public string level_name { get; private set; } = null!;

    public bool is_deleted { get; private set; }

    [InverseProperty("confirmed_care_level")]
    public virtual ICollection<assessment> assessmentconfirmed_care_levels { get; private set; } = new List<assessment>();

    [InverseProperty("suggested_care_level")]
    public virtual ICollection<assessment> assessmentsuggested_care_levels { get; private set; } = new List<assessment>();

    [InverseProperty("care_level")]
    public virtual ICollection<care_level_rate> care_level_rates { get; private set; } = new List<care_level_rate>();

    [InverseProperty("care_level")]
    public virtual ICollection<resident_care_level_history> resident_care_level_histories { get; private set; } = new List<resident_care_level_history>();
}
