using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("medication_logs")]
[Index("AdministeredBy", Name = "idx_medication_logs_administered_by")]
[Index("OrderId", "LoggedAt", Name = "idx_medication_logs_order_id_logged_at")]
public partial class MedicationLog
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("is_clinically_justified")]
    public bool IsClinicallyJustified { get; private set; }

    [Column("override_reason")]
    [StringLength(500)]
    public string? OverrideReason { get; private set; }

    [Column("order_id")]
    public long OrderId { get; private set; }

    [Column("administered_by")]
    public long AdministeredBy { get; private set; }

    [Column("witnessed_by")]
    public long? WitnessedBy { get; private set; }

    [Column("logged_at")]
    [Precision(0)]
    public DateTimeOffset LoggedAt { get; private set; }

    [ForeignKey("AdministeredBy")]
    [InverseProperty("MedicationLogAdministeredByNavigations")]
    public virtual User AdministeredByNavigation { get; private set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("MedicationLogs")]
    public virtual MedicationOrder Order { get; private set; } = null!;

    [ForeignKey("WitnessedBy")]
    [InverseProperty("MedicationLogWitnessedByNavigations")]
    public virtual User? WitnessedByNavigation { get; private set; }
}
