using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("consumable_supplies")]
[Index("FacilityId", "IsDeleted", Name = "idx_supplies_facility_lookup")]
[Index("Status", Name = "idx_supplies_status_filter")]
public partial class ConsumableSupply
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("item_name")]
    [StringLength(200)]
    public string ItemName { get; private set; } = null!;

    [Column("category_id")]
    public long CategoryId { get; private set; }

    [Column("facility_id")]
    public long FacilityId { get; private set; }

    [Column("stock_on_hand")]
    public int StockOnHand { get; private set; }

    [Column("total")]
    public int Total { get; private set; }

    [Column("reorder_threshold")]
    public int ReorderThreshold { get; private set; }

    [Column("unit_cost", TypeName = "decimal(18, 2)")]
    public decimal UnitCost { get; private set; }

    [Column("private_pay_rate", TypeName = "decimal(18, 2)")]
    public decimal PrivatePayRate { get; private set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [Column("updated_at")]
    [Precision(0)]
    public DateTimeOffset UpdatedAt { get; private set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("ConsumableSupplies")]
    public virtual InventoryCategory Category { get; private set; } = null!;

    [ForeignKey("FacilityId")]
    [InverseProperty("ConsumableSupplies")]
    public virtual Facility Facility { get; private set; } = null!;
}
