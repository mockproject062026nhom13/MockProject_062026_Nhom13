using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("resident_insurance_policies")]
[Index("ResidentId", Name = "idx_resident_insurance_policies_resident_id")]
public partial class ResidentInsurancePolicy
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("resident_id")]
    public long ResidentId { get; private set; }

    [Column("insurance_provider_id")]
    public long InsuranceProviderId { get; private set; }

    [Column("policy_number_encrypted")]
    [StringLength(512)]
    [Unicode(false)]
    public string PolicyNumberEncrypted { get; private set; } = null!;

    [Column("group_number")]
    [StringLength(100)]
    public string? GroupNumber { get; private set; }

    [Column("effective_from")]
    public DateOnly EffectiveFrom { get; private set; }

    [Column("effective_to")]
    public DateOnly? EffectiveTo { get; private set; }

    [Column("is_primary")]
    public bool IsPrimary { get; private set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [ForeignKey("InsuranceProviderId")]
    [InverseProperty("ResidentInsurancePolicies")]
    public virtual InsuranceProvider InsuranceProvider { get; private set; } = null!;

    [ForeignKey("ResidentId")]
    [InverseProperty("ResidentInsurancePolicies")]
    public virtual Resident Resident { get; private set; } = null!;
}
