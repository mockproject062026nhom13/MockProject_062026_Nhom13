using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("addresses")]
public partial class Address
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("street_line1")]
    [StringLength(200)]
    public string StreetLine1 { get; private set; } = null!;

    [Column("street_line2")]
    [StringLength(200)]
    public string? StreetLine2 { get; private set; }

    [Column("city")]
    [StringLength(100)]
    public string City { get; private set; } = null!;

    [Column("state")]
    [StringLength(2)]
    [Unicode(false)]
    public string State { get; private set; } = null!;

    [Column("zip_code")]
    [StringLength(10)]
    [Unicode(false)]
    public string ZipCode { get; private set; } = null!;

    [Column("address_type")]
    [StringLength(20)]
    [Unicode(false)]
    public string AddressType { get; private set; } = null!;

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [Column("updated_at")]
    [Precision(0)]
    public DateTimeOffset UpdatedAt { get; private set; }

    [InverseProperty("Address")]
    public virtual ICollection<Contact> Contacts { get; private set; } = new List<Contact>();

    [InverseProperty("Address")]
    public virtual ICollection<Facility> Facilities { get; private set; } = new List<Facility>();

    [InverseProperty("Address")]
    public virtual ICollection<Resident> Residents { get; private set; } = new List<Resident>();
}
