using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("resident_id", Name = "idx_medication_orders_resident_id")]
[Index("status", Name = "idx_medication_orders_status")]
public partial class medication_order
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(200)]
    public string drug_name { get; private set; } = null!;

    [StringLength(100)]
    public string dosage { get; private set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string route { get; private set; } = null!;

    [StringLength(100)]
    public string frequency { get; private set; } = null!;

    public bool is_controlled_substance { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string status { get; private set; } = null!;

    public Guid resident_id { get; private set; }

    public Guid prescribed_by { get; private set; }

    public bool is_deleted { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset updated_at { get; private set; }

    [InverseProperty("order")]
    public virtual ICollection<medication_log> medication_logs { get; private set; } = new List<medication_log>();

    [InverseProperty("order")]
    public virtual ICollection<medication_schedule> medication_schedules { get; private set; } = new List<medication_schedule>();

    [ForeignKey("prescribed_by")]
    [InverseProperty("medication_orders")]
    public virtual user prescribed_byNavigation { get; private set; } = null!;

    [ForeignKey("resident_id")]
    [InverseProperty("medication_orders")]
    public virtual resident resident { get; private set; } = null!;
}
