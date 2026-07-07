using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("contact_id", Name = "idx_resident_contacts_contact_id")]
[Index("resident_id", Name = "idx_resident_contacts_resident_id")]
[Index("resident_id", "contact_id", "relationship_type", Name = "uq_resident_contact", IsUnique = true)]
public partial class resident_contact
{
    [Key]
    public long id { get; private set; }

    public Guid resident_id { get; private set; }

    public Guid contact_id { get; private set; }

    [StringLength(50)]
    [Unicode(false)]
    public string relationship_type { get; private set; } = null!;

    public bool is_guarantor { get; private set; }

    public bool is_emergency_contact { get; private set; }

    public bool is_primary { get; private set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? financial_responsibility_pct { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [ForeignKey("contact_id")]
    [InverseProperty("resident_contacts")]
    public virtual contact contact { get; private set; } = null!;

    [ForeignKey("resident_id")]
    [InverseProperty("resident_contacts")]
    public virtual resident resident { get; private set; } = null!;
}
