using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("resident_contacts")]
[Index("ContactId", Name = "idx_resident_contacts_contact_id")]
[Index("ResidentId", Name = "idx_resident_contacts_resident_id")]
[Index("ResidentId", "ContactId", "RelationshipType", Name = "uq_resident_contact", IsUnique = true)]
public partial class ResidentContact
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("resident_id")]
    public long ResidentId { get; private set; }

    [Column("contact_id")]
    public long ContactId { get; private set; }

    [Column("relationship_type")]
    [StringLength(50)]
    [Unicode(false)]
    public string RelationshipType { get; private set; } = null!;

    [Column("is_guarantor")]
    public bool IsGuarantor { get; private set; }

    [Column("is_emergency_contact")]
    public bool IsEmergencyContact { get; private set; }

    [Column("is_primary")]
    public bool IsPrimary { get; private set; }

    [Column("financial_responsibility_pct", TypeName = "decimal(5, 2)")]
    public decimal? FinancialResponsibilityPct { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [ForeignKey("ContactId")]
    [InverseProperty("ResidentContacts")]
    public virtual Contact Contact { get; private set; } = null!;

    [ForeignKey("ResidentId")]
    [InverseProperty("ResidentContacts")]
    public virtual Resident Resident { get; private set; } = null!;
}
