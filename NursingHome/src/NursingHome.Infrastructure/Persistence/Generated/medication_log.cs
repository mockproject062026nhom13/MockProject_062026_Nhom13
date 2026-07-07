using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("administered_by", Name = "idx_medication_logs_administered_by")]
[Index("order_id", "logged_at", Name = "idx_medication_logs_order_id_logged_at", IsDescending = new[] { false, true })]
public partial class medication_log
{
    [Key]
    public long id { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string status { get; private set; } = null!;

    public bool is_clinically_justified { get; private set; }

    [StringLength(500)]
    public string? override_reason { get; private set; }

    public Guid order_id { get; private set; }

    public Guid administered_by { get; private set; }

    public Guid? witnessed_by { get; private set; }

    [Precision(0)]
    public DateTimeOffset logged_at { get; private set; }

    [ForeignKey("administered_by")]
    [InverseProperty("medication_logadministered_byNavigations")]
    public virtual user administered_byNavigation { get; private set; } = null!;

    [ForeignKey("order_id")]
    [InverseProperty("medication_logs")]
    public virtual medication_order order { get; private set; } = null!;

    [ForeignKey("witnessed_by")]
    [InverseProperty("medication_logwitnessed_byNavigations")]
    public virtual user? witnessed_byNavigation { get; private set; }
}
