using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

public partial class address
{
    [Key]
    public long id { get; private set; }

    [StringLength(200)]
    public string street_line1 { get; private set; } = null!;

    [StringLength(200)]
    public string? street_line2 { get; private set; }

    [StringLength(100)]
    public string city { get; private set; } = null!;

    [StringLength(2)]
    [Unicode(false)]
    public string state { get; private set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string zip_code { get; private set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string address_type { get; private set; } = null!;

    public bool is_deleted { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset updated_at { get; private set; }

    [InverseProperty("address")]
    public virtual ICollection<contact> contacts { get; private set; } = new List<contact>();

    [InverseProperty("address")]
    public virtual ICollection<facility> facilities { get; private set; } = new List<facility>();

    [InverseProperty("address")]
    public virtual ICollection<resident> residents { get; private set; } = new List<resident>();
}
