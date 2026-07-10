using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("invoice_line_items")]
[Index("InvoiceId", Name = "idx_invoice_line_items_invoice_id")]
public partial class InvoiceLineItem
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("invoice_id")]
    public long InvoiceId { get; private set; }

    [Column("description")]
    [StringLength(255)]
    public string Description { get; private set; } = null!;

    [Column("item_type")]
    [StringLength(30)]
    [Unicode(false)]
    public string ItemType { get; private set; } = null!;

    [Column("amount", TypeName = "decimal(18, 2)")]
    public decimal Amount { get; private set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("InvoiceLineItems")]
    public virtual Invoice Invoice { get; private set; } = null!;
}
