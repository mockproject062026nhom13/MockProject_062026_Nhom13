using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("audit_logs")]
[Index("PerformedAt", Name = "idx_audit_logs_performed_at")]
[Index("PerformedBy", Name = "idx_audit_logs_performed_by")]
[Index("TableName", "RecordId", Name = "idx_audit_logs_table_record")]
public partial class AuditLog
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("table_name")]
    [StringLength(100)]
    public string TableName { get; private set; } = null!;

    [Column("record_id")]
    [StringLength(100)]
    public string RecordId { get; private set; } = null!;

    [Column("action")]
    [StringLength(20)]
    [Unicode(false)]
    public string Action { get; private set; } = null!;

    [Column("old_data")]
    public string? OldData { get; private set; }

    [Column("new_data")]
    public string? NewData { get; private set; }

    [Column("performed_by")]
    public long PerformedBy { get; private set; }

    [Column("performed_at")]
    [Precision(0)]
    public DateTimeOffset PerformedAt { get; private set; }

    [Column("ip_address")]
    [StringLength(45)]
    [Unicode(false)]
    public string? IpAddress { get; private set; }

    [ForeignKey("PerformedBy")]
    [InverseProperty("AuditLogs")]
    public virtual User PerformedByNavigation { get; private set; } = null!;
}
