using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("action_code", Name = "UQ__permissi__BFFF1CB86F72BBCB", IsUnique = true)]
public partial class permission
{
    [Key]
    public long id { get; private set; }

    [StringLength(100)]
    public string action_code { get; private set; } = null!;

    public bool is_phi_sensitive { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [ForeignKey("permission_id")]
    [InverseProperty("permissions")]
    public virtual ICollection<role> roles { get; private set; } = new List<role>();
}
