using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("durable_medical_equipment")]
[Index("AssetTag", Name = "UQ__durable___1FACF043F5E12A32", IsUnique = true)]
[Index("FacilityId", "IsDeleted", Name = "idx_dme_facility_lookup")]
[Index("ItemName", Name = "idx_dme_item_name")]
[Index("Status", Name = "idx_dme_status_filter")]
public partial class DurableMedicalEquipment
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("item_name")]
    [StringLength(200)]
    public string ItemName { get; private set; } = null!;

    [Column("category_id")]
    public long CategoryId { get; private set; }

    [Column("asset_tag")]
    [StringLength(50)]
    [Unicode(false)]
    public string AssetTag { get; private set; } = null!;

    [Column("status")]
    [StringLength(30)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("facility_id")]
    public long FacilityId { get; private set; }

    [Column("assigned_to_user")]
    public long? AssignedToUser { get; private set; }

    [Column("assigned_to_resident")]
    public long? AssignedToResident { get; private set; }

    [Column("unit_value", TypeName = "decimal(18, 2)")]
    public decimal UnitValue { get; private set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [Column("updated_at")]
    [Precision(0)]
    public DateTimeOffset UpdatedAt { get; private set; }

    [ForeignKey("AssignedToResident")]
    [InverseProperty("DurableMedicalEquipments")]
    public virtual Resident? AssignedToResidentNavigation { get; private set; }

    [ForeignKey("AssignedToUser")]
    [InverseProperty("DurableMedicalEquipments")]
    public virtual User? AssignedToUserNavigation { get; private set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("DurableMedicalEquipments")]
    public virtual InventoryCategory Category { get; private set; } = null!;

    [ForeignKey("FacilityId")]
    [InverseProperty("DurableMedicalEquipments")]
    public virtual Facility Facility { get; private set; } = null!;
}
