using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("invoice_id", Name = "idx_invoice_line_items_invoice_id")]
public partial class invoice_line_item
{
    [Key]
    public long id { get; private set; }

    public Guid invoice_id { get; private set; }

    [StringLength(255)]
    public string description { get; private set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string item_type { get; private set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal amount { get; private set; }

    [ForeignKey("invoice_id")]
    [InverseProperty("invoice_line_items")]
    public virtual invoice invoice { get; private set; } = null!;
}
