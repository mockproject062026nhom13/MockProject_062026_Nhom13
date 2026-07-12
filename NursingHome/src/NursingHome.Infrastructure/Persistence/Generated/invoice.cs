using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("invoices")]
[Index("ResidentId", Name = "idx_invoices_resident_id")]
[Index("Status", Name = "idx_invoices_status")]
public partial class Invoice
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("resident_id")]
    public long ResidentId { get; private set; }

    [Column("billing_period_start")]
    public DateOnly BillingPeriodStart { get; private set; }

    [Column("billing_period_end")]
    public DateOnly BillingPeriodEnd { get; private set; }

    [Column("total_amount", TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; private set; }

    [Column("medicare_covered_amount", TypeName = "decimal(18, 2)")]
    public decimal MedicareCoveredAmount { get; private set; }

    [Column("medicaid_covered_amount", TypeName = "decimal(18, 2)")]
    public decimal MedicaidCoveredAmount { get; private set; }

    [Column("private_insurance_covered_amount", TypeName = "decimal(18, 2)")]
    public decimal PrivateInsuranceCoveredAmount { get; private set; }

    [Column("patient_responsibility_amount", TypeName = "decimal(18, 2)")]
    public decimal PatientResponsibilityAmount { get; private set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("due_date")]
    public DateOnly DueDate { get; private set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [Column("updated_at")]
    [Precision(0)]
    public DateTimeOffset UpdatedAt { get; private set; }

    [InverseProperty("Invoice")]
    public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; private set; } = new List<InvoiceLineItem>();

    [InverseProperty("Invoice")]
    public virtual ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    [ForeignKey("ResidentId")]
    [InverseProperty("Invoices")]
    public virtual Resident Resident { get; private set; } = null!;
}
