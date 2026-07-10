using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("permissions")]
[Index("ActionCode", Name = "UQ__permissi__BFFF1CB8F6F010F2", IsUnique = true)]
public partial class Permission
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("action_code")]
    [StringLength(100)]
    public string ActionCode { get; private set; } = null!;

    [Column("is_phi_sensitive")]
    public bool IsPhiSensitive { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [ForeignKey("PermissionId")]
    [InverseProperty("Permissions")]
    public virtual ICollection<Role> Roles { get; private set; } = new List<Role>();
}
