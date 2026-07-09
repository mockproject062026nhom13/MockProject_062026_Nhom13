using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("notifications")]
[Index("UserId", "IsRead", Name = "idx_notifications_user_id_is_read")]
public partial class Notification
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; private set; } = null!;

    [Column("type")]
    [StringLength(50)]
    [Unicode(false)]
    public string Type { get; private set; } = null!;

    [Column("is_read")]
    public bool IsRead { get; private set; }

    [Column("user_id")]
    public long UserId { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [ForeignKey("UserId")]
    [InverseProperty("Notifications")]
    public virtual User User { get; private set; } = null!;
}
