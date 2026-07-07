using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[PrimaryKey("user_id", "facility_id")]
public partial class user_facility
{
    [Key]
    public Guid user_id { get; private set; }

    [Key]
    public Guid facility_id { get; private set; }

    public bool is_primary { get; private set; }

    [ForeignKey("facility_id")]
    [InverseProperty("user_facilities")]
    public virtual facility facility { get; private set; } = null!;

    [ForeignKey("user_id")]
    [InverseProperty("user_facilities")]
    public virtual user user { get; private set; } = null!;
}
