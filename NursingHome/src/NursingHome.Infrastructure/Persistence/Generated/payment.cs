using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("payments")]
[Index("InvoiceId", Name = "idx_payments_invoice_id")]
public partial class Payment
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("invoice_id")]
    public long InvoiceId { get; private set; }

    [Column("payer_type")]
    [StringLength(20)]
    [Unicode(false)]
    public string PayerType { get; private set; } = null!;

    [Column("payment_method")]
    [StringLength(20)]
    [Unicode(false)]
    public string PaymentMethod { get; private set; } = null!;

    [Column("amount", TypeName = "decimal(18, 2)")]
    public decimal Amount { get; private set; }

    [Column("payment_token_encrypted")]
    [StringLength(512)]
    [Unicode(false)]
    public string? PaymentTokenEncrypted { get; private set; }

    [Column("received_by")]
    public long ReceivedBy { get; private set; }

    [Column("paid_at")]
    [Precision(0)]
    public DateTimeOffset PaidAt { get; private set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("Payments")]
    public virtual Invoice Invoice { get; private set; } = null!;

    [ForeignKey("ReceivedBy")]
    [InverseProperty("Payments")]
    public virtual User ReceivedByNavigation { get; private set; } = null!;
}
