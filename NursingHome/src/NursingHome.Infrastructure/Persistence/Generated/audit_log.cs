using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("performed_at", Name = "idx_audit_logs_performed_at")]
[Index("performed_by", Name = "idx_audit_logs_performed_by")]
[Index("table_name", "record_id", Name = "idx_audit_logs_table_record")]
public partial class audit_log
{
    [Key]
    public long id { get; private set; }

    [StringLength(100)]
    public string table_name { get; private set; } = null!;

    [StringLength(100)]
    public string record_id { get; private set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string action { get; private set; } = null!;

    public string? old_data { get; private set; }

    public string? new_data { get; private set; }

    public Guid performed_by { get; private set; }

    [Precision(0)]
    public DateTimeOffset performed_at { get; private set; }

    [StringLength(45)]
    [Unicode(false)]
    public string? ip_address { get; private set; }

    [ForeignKey("performed_by")]
    [InverseProperty("audit_logs")]
    public virtual user performed_byNavigation { get; private set; } = null!;
}
