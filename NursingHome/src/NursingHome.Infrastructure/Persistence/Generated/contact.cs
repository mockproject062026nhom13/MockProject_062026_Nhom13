using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class contact
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(100)]
    public string first_name { get; private set; } = null!;

    [StringLength(100)]
    public string? middle_name { get; private set; }

    [StringLength(100)]
    public string last_name { get; private set; } = null!;

    [StringLength(20)]
    public string phone_primary { get; private set; } = null!;

    [StringLength(20)]
    public string? phone_secondary { get; private set; }

    [StringLength(255)]
    public string? email { get; private set; }

    public long? address_id { get; private set; }

    public bool is_deleted { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset updated_at { get; private set; }

    [ForeignKey("address_id")]
    [InverseProperty("contacts")]
    public virtual address? address { get; private set; }

    [InverseProperty("contact")]
    public virtual ICollection<resident_contact> resident_contacts { get; private set; } = new List<resident_contact>();
}
