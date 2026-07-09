using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[PrimaryKey("UserId", "FacilityId")]
[Table("user_facilities")]
public partial class UserFacility
{
    [Key]
    [Column("user_id")]
    public long UserId { get; private set; }

    [Key]
    [Column("facility_id")]
    public long FacilityId { get; private set; }

    [Column("is_primary")]
    public bool IsPrimary { get; private set; }

    [ForeignKey("FacilityId")]
    [InverseProperty("UserFacilities")]
    public virtual Facility Facility { get; private set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserFacilities")]
    public virtual User User { get; private set; } = null!;
}
