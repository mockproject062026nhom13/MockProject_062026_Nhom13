using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("resident_sensitive_info")]
[Index("resident_id", Name = "UQ__resident__A5BC2ECF7E029494", IsUnique = true)]
public partial class resident_sensitive_info
{
    [Key]
    public long id { get; private set; }

    public Guid resident_id { get; private set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? ssn_encrypted { get; private set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? medical_record_number_encrypted { get; private set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? primary_insurance_id_encrypted { get; private set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? bank_account_encrypted { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset updated_at { get; private set; }

    [ForeignKey("resident_id")]
    [InverseProperty("resident_sensitive_info")]
    public virtual resident resident { get; private set; } = null!;
}
