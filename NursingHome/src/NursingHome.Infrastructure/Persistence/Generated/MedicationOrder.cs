using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("medication_orders")]
[Index("ResidentId", Name = "idx_medication_orders_resident_id")]
[Index("Status", Name = "idx_medication_orders_status")]
public partial class MedicationOrder
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("drug_name")]
    [StringLength(200)]
    public string DrugName { get; private set; } = null!;

    [Column("dosage")]
    [StringLength(100)]
    public string Dosage { get; private set; } = null!;

    [Column("route")]
    [StringLength(30)]
    [Unicode(false)]
    public string Route { get; private set; } = null!;

    [Column("frequency")]
    [StringLength(100)]
    public string Frequency { get; private set; } = null!;

    [Column("is_controlled_substance")]
    public bool IsControlledSubstance { get; private set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("resident_id")]
    public long ResidentId { get; private set; }

    [Column("prescribed_by")]
    public long PrescribedBy { get; private set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [Column("updated_at")]
    [Precision(0)]
    public DateTimeOffset UpdatedAt { get; private set; }

    [InverseProperty("Order")]
    public virtual ICollection<MedicationLog> MedicationLogs { get; private set; } = new List<MedicationLog>();

    [InverseProperty("Order")]
    public virtual ICollection<MedicationSchedule> MedicationSchedules { get; private set; } = new List<MedicationSchedule>();

    [ForeignKey("PrescribedBy")]
    [InverseProperty("MedicationOrders")]
    public virtual User PrescribedByNavigation { get; private set; } = null!;

    [ForeignKey("ResidentId")]
    [InverseProperty("MedicationOrders")]
    public virtual Resident Resident { get; private set; } = null!;
}
