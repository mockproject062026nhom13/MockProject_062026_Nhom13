using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("contacts")]
public partial class Contact
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("first_name")]
    [StringLength(100)]
    public string FirstName { get; private set; } = null!;

    [Column("middle_name")]
    [StringLength(100)]
    public string? MiddleName { get; private set; }

    [Column("last_name")]
    [StringLength(100)]
    public string LastName { get; private set; } = null!;

    [Column("phone_primary")]
    [StringLength(20)]
    public string PhonePrimary { get; private set; } = null!;

    [Column("phone_secondary")]
    [StringLength(20)]
    public string? PhoneSecondary { get; private set; }

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; private set; }

    [Column("address_id")]
    public long? AddressId { get; private set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [Column("updated_at")]
    [Precision(0)]
    public DateTimeOffset UpdatedAt { get; private set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Contacts")]
    public virtual Address? Address { get; private set; }

    [InverseProperty("Contact")]
    public virtual ICollection<ResidentContact> ResidentContacts { get; private set; } = new List<ResidentContact>();
}
