using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("phi_access_logs")]
[Index("AccessedBy", Name = "idx_phi_access_logs_accessed_by")]
[Index("TableName", "RecordId", Name = "idx_phi_access_logs_table_record")]
public partial class PhiAccessLog
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

    [Column("accessed_by")]
    public long AccessedBy { get; private set; }

    [Column("access_type")]
    [StringLength(20)]
    [Unicode(false)]
    public string AccessType { get; private set; } = null!;

    [Column("access_reason")]
    [StringLength(255)]
    public string? AccessReason { get; private set; }

    [Column("ip_address")]
    [StringLength(45)]
    [Unicode(false)]
    public string? IpAddress { get; private set; }

    [Column("accessed_at")]
    [Precision(0)]
    public DateTimeOffset AccessedAt { get; private set; }

    [ForeignKey("AccessedBy")]
    [InverseProperty("PhiAccessLogs")]
    public virtual User AccessedByNavigation { get; private set; } = null!;
}
