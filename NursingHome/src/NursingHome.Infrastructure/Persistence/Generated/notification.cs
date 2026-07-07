using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("user_id", "is_read", Name = "idx_notifications_user_id_is_read")]
public partial class notification
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(255)]
    public string title { get; private set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string type { get; private set; } = null!;

    public bool is_read { get; private set; }

    public Guid user_id { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [ForeignKey("user_id")]
    [InverseProperty("notifications")]
    public virtual user user { get; private set; } = null!;
}
