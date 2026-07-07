using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("bed_id", Name = "idx_residents_bed_id")]
[Index("date_of_birth", Name = "idx_residents_dob")]
[Index("last_name", "first_name", Name = "idx_residents_name")]
public partial class resident
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(100)]
    public string first_name { get; private set; } = null!;

    [StringLength(100)]
    public string? middle_name { get; private set; }

    [StringLength(100)]
    public string last_name { get; private set; } = null!;

    public DateOnly date_of_birth { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? gender { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? marital_status { get; private set; }

    [StringLength(100)]
    public string? religion_preference { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string status { get; private set; } = null!;

    public bool is_chart_locked { get; private set; }

    public long? address_id { get; private set; }

    public Guid? bed_id { get; private set; }

    public bool is_deleted { get; private set; }

    [Precision(0)]
    public DateTimeOffset? deleted_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [Precision(0)]
    public DateTimeOffset updated_at { get; private set; }

    [ForeignKey("address_id")]
    [InverseProperty("residents")]
    public virtual address? address { get; private set; }

    [InverseProperty("resident")]
    public virtual ICollection<admission> admissions { get; private set; } = new List<admission>();

    [InverseProperty("resident")]
    public virtual ICollection<assessment> assessments { get; private set; } = new List<assessment>();

    [ForeignKey("bed_id")]
    [InverseProperty("residents")]
    public virtual bed? bed { get; private set; }

    [InverseProperty("resident")]
    public virtual ICollection<care_plan> care_plans { get; private set; } = new List<care_plan>();

    [InverseProperty("resident")]
    public virtual ICollection<clinical_record> clinical_records { get; private set; } = new List<clinical_record>();

    [InverseProperty("resident")]
    public virtual ICollection<incident> incidents { get; private set; } = new List<incident>();

    [InverseProperty("resident")]
    public virtual ICollection<invoice> invoices { get; private set; } = new List<invoice>();

    [InverseProperty("resident")]
    public virtual ICollection<medication_order> medication_orders { get; private set; } = new List<medication_order>();

    [InverseProperty("resident")]
    public virtual ICollection<pre_admission_screening> pre_admission_screenings { get; private set; } = new List<pre_admission_screening>();

    [InverseProperty("resident")]
    public virtual ICollection<resident_care_level_history> resident_care_level_histories { get; private set; } = new List<resident_care_level_history>();

    [InverseProperty("resident")]
    public virtual ICollection<resident_contact> resident_contacts { get; private set; } = new List<resident_contact>();

    [InverseProperty("resident")]
    public virtual ICollection<resident_insurance_policy> resident_insurance_policies { get; private set; } = new List<resident_insurance_policy>();

    [InverseProperty("resident")]
    public virtual resident_sensitive_info? resident_sensitive_info { get; private set; }

    [InverseProperty("resident")]
    public virtual ICollection<vital_sign> vital_signs { get; private set; } = new List<vital_sign>();
}
