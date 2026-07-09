using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("roles")]
[Index("RoleName", Name = "UQ__roles__783254B186FE301E", IsUnique = true)]
public partial class Role
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("role_name")]
    [StringLength(100)]
    public string RoleName { get; private set; } = null!;

    [Column("description")]
    [StringLength(500)]
    public string? Description { get; private set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [Column("updated_at")]
    [Precision(0)]
    public DateTimeOffset UpdatedAt { get; private set; }

    [InverseProperty("Role")]
    public virtual ICollection<User> Users { get; private set; } = new List<User>();

    [ForeignKey("RoleId")]
    [InverseProperty("Roles")]
    public virtual ICollection<Permission> Permissions { get; private set; } = new List<Permission>();
}
