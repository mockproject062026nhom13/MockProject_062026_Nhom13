using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("role_name", Name = "UQ__roles__783254B138792400", IsUnique = true)]
public partial class role
{
    [Key]
    public long id { get; private set; }

    [StringLength(100)]
    public string role_name { get; private set; } = null!;

    [StringLength(500)]
    public string? description { get; private set; }

    public bool is_deleted { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset updated_at { get; private set; }

    [InverseProperty("role")]
    public virtual ICollection<user> users { get; private set; } = new List<user>();

    [ForeignKey("role_id")]
    [InverseProperty("roles")]
    public virtual ICollection<permission> permissions { get; private set; } = new List<permission>();
}
