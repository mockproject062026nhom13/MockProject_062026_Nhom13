using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("accessed_by", Name = "idx_phi_access_logs_accessed_by")]
[Index("table_name", "record_id", Name = "idx_phi_access_logs_table_record")]
public partial class phi_access_log
{
    [Key]
    public long id { get; private set; }

    [StringLength(100)]
    public string table_name { get; private set; } = null!;

    [StringLength(100)]
    public string record_id { get; private set; } = null!;

    public Guid accessed_by { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string access_type { get; private set; } = null!;

    [StringLength(255)]
    public string? access_reason { get; private set; }

    [StringLength(45)]
    [Unicode(false)]
    public string? ip_address { get; private set; }

    [Precision(0)]
    public DateTimeOffset accessed_at { get; private set; }

    [ForeignKey("accessed_by")]
    [InverseProperty("phi_access_logs")]
    public virtual user accessed_byNavigation { get; private set; } = null!;
}
