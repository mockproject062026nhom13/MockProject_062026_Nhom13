using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("resident_id", Name = "idx_resident_insurance_policies_resident_id")]
public partial class resident_insurance_policy
{
    [Key]
    public Guid id { get; private set; }

    public Guid resident_id { get; private set; }

    public long insurance_provider_id { get; private set; }

    [StringLength(512)]
    [Unicode(false)]
    public string policy_number_encrypted { get; private set; } = null!;

    [StringLength(100)]
    public string? group_number { get; private set; }

    public DateOnly effective_from { get; private set; }

    public DateOnly? effective_to { get; private set; }

    public bool is_primary { get; private set; }

    public bool is_deleted { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [ForeignKey("insurance_provider_id")]
    [InverseProperty("resident_insurance_policies")]
    public virtual insurance_provider insurance_provider { get; private set; } = null!;

    [ForeignKey("resident_id")]
    [InverseProperty("resident_insurance_policies")]
    public virtual resident resident { get; private set; } = null!;
}
