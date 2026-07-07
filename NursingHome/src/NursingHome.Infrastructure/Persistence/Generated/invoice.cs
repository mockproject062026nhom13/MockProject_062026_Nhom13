using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("resident_id", Name = "idx_invoices_resident_id")]
[Index("status", Name = "idx_invoices_status")]
public partial class invoice
{
    [Key]
    public Guid id { get; private set; }

    public Guid resident_id { get; private set; }

    public DateOnly billing_period_start { get; private set; }

    public DateOnly billing_period_end { get; private set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal total_amount { get; private set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal medicare_covered_amount { get; private set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal medicaid_covered_amount { get; private set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal private_insurance_covered_amount { get; private set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal patient_responsibility_amount { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string status { get; private set; } = null!;

    public DateOnly due_date { get; private set; }

    public bool is_deleted { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset updated_at { get; private set; }

    [InverseProperty("invoice")]
    public virtual ICollection<invoice_line_item> invoice_line_items { get; private set; } = new List<invoice_line_item>();

    [InverseProperty("invoice")]
    public virtual ICollection<payment> payments { get; private set; } = new List<payment>();

    [ForeignKey("resident_id")]
    [InverseProperty("invoices")]
    public virtual resident resident { get; private set; } = null!;
}
