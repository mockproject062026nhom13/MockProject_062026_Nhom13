using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("inventory_categories")]
[Index("CategoryName", Name = "UQ__inventor__5189E25500D8C6D6", IsUnique = true)]
public partial class InventoryCategory
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("category_name")]
    [StringLength(100)]
    public string CategoryName { get; private set; } = null!;

    [Column("description")]
    [StringLength(500)]
    public string? Description { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [InverseProperty("Category")]
    public virtual ICollection<ConsumableSupply> ConsumableSupplies { get; private set; } = new List<ConsumableSupply>();

    [InverseProperty("Category")]
    public virtual ICollection<DurableMedicalEquipment> DurableMedicalEquipments { get; private set; } = new List<DurableMedicalEquipment>();
}
