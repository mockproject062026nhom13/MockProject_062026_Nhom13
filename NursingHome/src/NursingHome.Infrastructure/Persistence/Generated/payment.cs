using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("invoice_id", Name = "idx_payments_invoice_id")]
public partial class payment
{
    [Key]
    public Guid id { get; private set; }

    public Guid invoice_id { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string payer_type { get; private set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string payment_method { get; private set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal amount { get; private set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? payment_token_encrypted { get; private set; }

    public Guid received_by { get; private set; }

    [Precision(0)]
    public DateTimeOffset paid_at { get; private set; }

    [ForeignKey("invoice_id")]
    [InverseProperty("payments")]
    public virtual invoice invoice { get; private set; } = null!;

    [ForeignKey("received_by")]
    [InverseProperty("payments")]
    public virtual user received_byNavigation { get; private set; } = null!;
}
