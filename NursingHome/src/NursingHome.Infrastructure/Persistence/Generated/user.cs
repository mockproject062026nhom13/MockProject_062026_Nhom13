using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("users")]
[Index("Email", Name = "UQ__users__AB6E61645623D15D", IsUnique = true)]
[Index("EmployeeCode", Name = "UQ__users__B0AA73456810CD46", IsUnique = true)]
[Index("RoleId", Name = "idx_users_role_id")]
[Index("Status", Name = "idx_users_status")]
public partial class User
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("employee_code")]
    [StringLength(50)]
    public string EmployeeCode { get; private set; } = null!;

    [Column("email")]
    [StringLength(255)]
    public string Email { get; private set; } = null!;

    [Column("password_hash")]
    [StringLength(300)]
    public string PasswordHash { get; private set; } = null!;

    [Column("first_name")]
    [StringLength(100)]
    public string FirstName { get; private set; } = null!;

    [Column("middle_name")]
    [StringLength(100)]
    public string? MiddleName { get; private set; }

    [Column("last_name")]
    [StringLength(100)]
    public string LastName { get; private set; } = null!;

    [Column("license_number")]
    [StringLength(100)]
    public string? LicenseNumber { get; private set; }

    [Column("phone_number")]
    [StringLength(20)]
    public string? PhoneNumber { get; private set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("mfa_enabled")]
    public bool MfaEnabled { get; private set; }

    [Column("last_login_at")]
    [Precision(0)]
    public DateTimeOffset? LastLoginAt { get; private set; }

    [Column("role_id")]
    public long RoleId { get; private set; }

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

    [InverseProperty("AssessedByNavigation")]
    public virtual ICollection<Assessment> Assessments { get; private set; } = new List<Assessment>();

    [InverseProperty("PerformedByNavigation")]
    public virtual ICollection<AuditLog> AuditLogs { get; private set; } = new List<AuditLog>();

    [InverseProperty("AssignedCna")]
    public virtual ICollection<CareTask> CareTasks { get; private set; } = new List<CareTask>();

    [InverseProperty("RecordedByNavigation")]
    public virtual ICollection<ClinicalRecord> ClinicalRecords { get; private set; } = new List<ClinicalRecord>();

    [InverseProperty("AssignedToUserNavigation")]
    public virtual ICollection<DurableMedicalEquipment> DurableMedicalEquipments { get; private set; } = new List<DurableMedicalEquipment>();

    [InverseProperty("ActorNavigation")]
    public virtual ICollection<IncidentTimeline> IncidentTimelines { get; private set; } = new List<IncidentTimeline>();

    [InverseProperty("ReportedByNavigation")]
    public virtual ICollection<Incident> Incidents { get; private set; } = new List<Incident>();

    [InverseProperty("AdministeredByNavigation")]
    public virtual ICollection<MedicationLog> MedicationLogAdministeredByNavigations { get; private set; } = new List<MedicationLog>();

    [InverseProperty("WitnessedByNavigation")]
    public virtual ICollection<MedicationLog> MedicationLogWitnessedByNavigations { get; private set; } = new List<MedicationLog>();

    [InverseProperty("PrescribedByNavigation")]
    public virtual ICollection<MedicationOrder> MedicationOrders { get; private set; } = new List<MedicationOrder>();

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; private set; } = new List<Notification>();

    [InverseProperty("ReceivedByNavigation")]
    public virtual ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    [InverseProperty("AccessedByNavigation")]
    public virtual ICollection<PhiAccessLog> PhiAccessLogs { get; private set; } = new List<PhiAccessLog>();

    [InverseProperty("ScreenedByNavigation")]
    public virtual ICollection<PreAdmissionScreening> PreAdmissionScreenings { get; private set; } = new List<PreAdmissionScreening>();

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; private set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<ShiftAssignment> ShiftAssignments { get; private set; } = new List<ShiftAssignment>();

    [InverseProperty("User")]
    public virtual ICollection<UserFacility> UserFacilities { get; private set; } = new List<UserFacility>();

    [InverseProperty("RecordedByNavigation")]
    public virtual ICollection<VitalSign> VitalSigns { get; private set; } = new List<VitalSign>();


    public void UpdateLastLogin(DateTimeOffset time)
    {
        this.LastLoginAt = time;
    }
}
