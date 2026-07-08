using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("email", Name = "UQ__users__AB6E6164E8C6DA5D", IsUnique = true)]
[Index("employee_code", Name = "UQ__users__B0AA7345BD7A3582", IsUnique = true)]
[Index("role_id", Name = "idx_users_role_id")]
public partial class user
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(50)]
    public string employee_code { get; private set; } = null!;

    [StringLength(255)]
    public string email { get; private set; } = null!;

    [StringLength(300)]
    public string password_hash { get; private set; } = null!;

    [StringLength(100)]
    public string first_name { get; private set; } = null!;

    [StringLength(100)]
    public string? middle_name { get; private set; }

    [StringLength(100)]
    public string last_name { get; private set; } = null!;

    [StringLength(100)]
    public string? license_number { get; private set; }

    [StringLength(20)]
    public string? phone_number { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string status { get; private set; } = null!;

    public bool mfa_enabled { get; private set; }

    [Precision(0)]
    public DateTimeOffset? last_login_at { get; private set; }

    public long role_id { get; private set; }

    public bool is_deleted { get; private set; }

    [Precision(0)]
    public DateTimeOffset? deleted_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset updated_at { get; private set; }

    [InverseProperty("assessed_byNavigation")]
    public virtual ICollection<assessment> assessments { get; private set; } = new List<assessment>();

    [InverseProperty("performed_byNavigation")]
    public virtual ICollection<audit_log> audit_logs { get; private set; } = new List<audit_log>();

    [InverseProperty("assigned_cna")]
    public virtual ICollection<care_task> care_tasks { get; private set; } = new List<care_task>();

    [InverseProperty("unlocked_byNavigation")]
    public virtual ICollection<chart_lock_event> chart_lock_events { get; private set; } = new List<chart_lock_event>();

    [InverseProperty("recorded_byNavigation")]
    public virtual ICollection<clinical_record> clinical_records { get; private set; } = new List<clinical_record>();

    [InverseProperty("reported_byNavigation")]
    public virtual ICollection<incident> incidents { get; private set; } = new List<incident>();

    [InverseProperty("administered_byNavigation")]
    public virtual ICollection<medication_log> medication_logadministered_byNavigations { get; private set; } = new List<medication_log>();

    [InverseProperty("witnessed_byNavigation")]
    public virtual ICollection<medication_log> medication_logwitnessed_byNavigations { get; private set; } = new List<medication_log>();

    [InverseProperty("prescribed_byNavigation")]
    public virtual ICollection<medication_order> medication_orders { get; private set; } = new List<medication_order>();

    [InverseProperty("user")]
    public virtual ICollection<notification> notifications { get; private set; } = new List<notification>();

    [InverseProperty("received_byNavigation")]
    public virtual ICollection<payment> payments { get; private set; } = new List<payment>();

    [InverseProperty("accessed_byNavigation")]
    public virtual ICollection<phi_access_log> phi_access_logs { get; private set; } = new List<phi_access_log>();

    [InverseProperty("screened_byNavigation")]
    public virtual ICollection<pre_admission_screening> pre_admission_screenings { get; private set; } = new List<pre_admission_screening>();

    [ForeignKey("role_id")]
    [InverseProperty("users")]
    public virtual role role { get; private set; } = null!;

    [InverseProperty("user")]
    public virtual ICollection<shift_assignment> shift_assignments { get; private set; } = new List<shift_assignment>();

    [InverseProperty("user")]
    public virtual ICollection<user_facility> user_facilities { get; private set; } = new List<user_facility>();

    [InverseProperty("recorded_byNavigation")]
    public virtual ICollection<vital_sign> vital_signs { get; private set; } = new List<vital_sign>();


}
