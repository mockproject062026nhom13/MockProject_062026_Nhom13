using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class insurance_provider
{
    [Key]
    public long id { get; private set; }

    [StringLength(200)]
    public string provider_name { get; private set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string provider_type { get; private set; } = null!;

    [InverseProperty("insurance_provider")]
    public virtual ICollection<resident_insurance_policy> resident_insurance_policies { get; private set; } = new List<resident_insurance_policy>();
}
