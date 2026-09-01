using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("insurance_providers")]
public partial class InsuranceProvider
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("provider_name")]
    [StringLength(200)]
    public string ProviderName { get; private set; } = null!;

    [Column("provider_type")]
    [StringLength(20)]
    [Unicode(false)]
    public string ProviderType { get; private set; } = null!;

    [InverseProperty("InsuranceProvider")]
    public virtual ICollection<ResidentInsurancePolicy> ResidentInsurancePolicies { get; private set; } = new List<ResidentInsurancePolicy>();
}
