using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("facility_code", Name = "UQ__faciliti__EC22450ABCEC2DAD", IsUnique = true)]
public partial class facility
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(50)]
    public string facility_code { get; private set; } = null!;

    [StringLength(200)]
    public string name { get; private set; } = null!;

    [StringLength(100)]
    public string license_number { get; private set; } = null!;

    [StringLength(2)]
    [Unicode(false)]
    public string target_state { get; private set; } = null!;

    public long? address_id { get; private set; }

    [StringLength(20)]
    public string? phone_number { get; private set; }

    public bool is_deleted { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset updated_at { get; private set; }

    [ForeignKey("address_id")]
    [InverseProperty("facilities")]
    public virtual address? address { get; private set; }

    [InverseProperty("facility")]
    public virtual ICollection<admission> admissions { get; private set; } = new List<admission>();

    [InverseProperty("facility")]
    public virtual ICollection<care_level_rate> care_level_rates { get; private set; } = new List<care_level_rate>();

    [InverseProperty("facility")]
    public virtual ICollection<room> rooms { get; private set; } = new List<room>();

    [InverseProperty("facility")]
    public virtual ICollection<shift> shifts { get; private set; } = new List<shift>();

    [InverseProperty("facility")]
    public virtual ICollection<staffing_config> staffing_configs { get; private set; } = new List<staffing_config>();

    [InverseProperty("facility")]
    public virtual ICollection<user_facility> user_facilities { get; private set; } = new List<user_facility>();
}
