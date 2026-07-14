using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("residents")]
[Index("BedId", Name = "idx_residents_bed_id")]
[Index("DateOfBirth", Name = "idx_residents_dob")]
[Index("LastName", "FirstName", Name = "idx_residents_name")]
[Index("Status", Name = "idx_residents_status")]
public partial class Resident
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("first_name")]
    [StringLength(100)]
    public string FirstName { get; private set; } = null!;

    [Column("middle_name")]
    [StringLength(100)]
    public string? MiddleName { get; private set; }

    [Column("last_name")]
    [StringLength(100)]
    public string LastName { get; private set; } = null!;

    [Column("date_of_birth")]
    public DateOnly DateOfBirth { get; private set; }

    [Column("gender")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Gender { get; private set; }

    [Column("marital_status")]
    [StringLength(20)]
    [Unicode(false)]
    public string? MaritalStatus { get; private set; }

    [Column("religion_preference")]
    [StringLength(100)]
    public string? ReligionPreference { get; private set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("is_chart_locked")]
    public bool IsChartLocked { get; private set; }

    [Column("address_id")]
    public long? AddressId { get; private set; }

    [Column("bed_id")]
    public long? BedId { get; private set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [Column("deleted_at")]
    [Precision(0)]
    public DateTimeOffset? DeletedAt { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [Column("updated_at")]
    [Precision(0)]
    public DateTimeOffset UpdatedAt { get; private set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Residents")]
    public virtual Address? Address { get; private set; }

    [InverseProperty("Resident")]
    public virtual ICollection<Admission> Admissions { get; private set; } = new List<Admission>();

    [InverseProperty("Resident")]
    public virtual ICollection<Assessment> Assessments { get; private set; } = new List<Assessment>();

    [ForeignKey("BedId")]
    [InverseProperty("Residents")]
    public virtual Bed? Bed { get; private set; }

    [InverseProperty("Resident")]
    public virtual ICollection<CarePlan> CarePlans { get; private set; } = new List<CarePlan>();

    [InverseProperty("Resident")]
    public virtual ICollection<ClinicalRecord> ClinicalRecords { get; private set; } = new List<ClinicalRecord>();

    [InverseProperty("AssignedToResidentNavigation")]
    public virtual ICollection<DurableMedicalEquipment> DurableMedicalEquipments { get; private set; } = new List<DurableMedicalEquipment>();

    [InverseProperty("Resident")]
    public virtual ICollection<Incident> Incidents { get; private set; } = new List<Incident>();

    [InverseProperty("Resident")]
    public virtual ICollection<Invoice> Invoices { get; private set; } = new List<Invoice>();

    [InverseProperty("Resident")]
    public virtual ICollection<MedicationOrder> MedicationOrders { get; private set; } = new List<MedicationOrder>();

    [InverseProperty("Resident")]
    public virtual ICollection<PreAdmissionScreening> PreAdmissionScreenings { get; private set; } = new List<PreAdmissionScreening>();

    [InverseProperty("Resident")]
    public virtual ICollection<ResidentCareLevelHistory> ResidentCareLevelHistories { get; private set; } = new List<ResidentCareLevelHistory>();

    [InverseProperty("Resident")]
    public virtual ICollection<ResidentContact> ResidentContacts { get; private set; } = new List<ResidentContact>();

    [InverseProperty("Resident")]
    public virtual ICollection<ResidentInsurancePolicy> ResidentInsurancePolicies { get; private set; } = new List<ResidentInsurancePolicy>();

    [InverseProperty("Resident")]
    public virtual ResidentSensitiveInfo? ResidentSensitiveInfo { get; private set; }

    [InverseProperty("Resident")]
    public virtual ICollection<VitalSign> VitalSigns { get; private set; } = new List<VitalSign>();
}
