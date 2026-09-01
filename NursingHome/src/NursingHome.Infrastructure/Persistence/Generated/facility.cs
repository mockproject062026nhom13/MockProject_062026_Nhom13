using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("facilities")]
[Index("FacilityCode", Name = "UQ__faciliti__EC22450A228CEEE7", IsUnique = true)]
public partial class Facility
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("facility_code")]
    [StringLength(50)]
    public string FacilityCode { get; private set; } = null!;

    [Column("name")]
    [StringLength(200)]
    public string Name { get; private set; } = null!;

    [Column("license_number")]
    [StringLength(100)]
    public string LicenseNumber { get; private set; } = null!;

    [Column("target_state")]
    [StringLength(2)]
    [Unicode(false)]
    public string TargetState { get; private set; } = null!;

    [Column("address_id")]
    public long? AddressId { get; private set; }

    [Column("phone_number")]
    [StringLength(20)]
    public string? PhoneNumber { get; private set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [Column("updated_at")]
    [Precision(0)]
    public DateTimeOffset UpdatedAt { get; private set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Facilities")]
    public virtual Address? Address { get; private set; }

    [InverseProperty("Facility")]
    public virtual ICollection<Admission> Admissions { get; private set; } = new List<Admission>();

    [InverseProperty("Facility")]
    public virtual ICollection<CareLevelRate> CareLevelRates { get; private set; } = new List<CareLevelRate>();

    [InverseProperty("Facility")]
    public virtual ICollection<ConsumableSupply> ConsumableSupplies { get; private set; } = new List<ConsumableSupply>();

    [InverseProperty("Facility")]
    public virtual ICollection<DurableMedicalEquipment> DurableMedicalEquipments { get; private set; } = new List<DurableMedicalEquipment>();

    [InverseProperty("Facility")]
    public virtual ICollection<Room> Rooms { get; private set; } = new List<Room>();

    [InverseProperty("Facility")]
    public virtual ICollection<Shift> Shifts { get; private set; } = new List<Shift>();

    [InverseProperty("Facility")]
    public virtual ICollection<StaffingConfig> StaffingConfigs { get; private set; } = new List<StaffingConfig>();

    [InverseProperty("Facility")]
    public virtual ICollection<UserFacility> UserFacilities { get; private set; } = new List<UserFacility>();
}
