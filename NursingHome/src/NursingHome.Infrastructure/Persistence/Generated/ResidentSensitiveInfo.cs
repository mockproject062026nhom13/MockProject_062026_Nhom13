using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("resident_sensitive_info")]
[Index("ResidentId", Name = "UQ__resident__A5BC2ECFA7D9C737", IsUnique = true)]
public partial class ResidentSensitiveInfo
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("resident_id")]
    public long ResidentId { get; private set; }

    [Column("ssn_encrypted")]
    [StringLength(512)]
    [Unicode(false)]
    public string? SsnEncrypted { get; private set; }

    [Column("medical_record_number_encrypted")]
    [StringLength(512)]
    [Unicode(false)]
    public string? MedicalRecordNumberEncrypted { get; private set; }

    [Column("primary_insurance_id_encrypted")]
    [StringLength(512)]
    [Unicode(false)]
    public string? PrimaryInsuranceIdEncrypted { get; private set; }

    [Column("bank_account_encrypted")]
    [StringLength(512)]
    [Unicode(false)]
    public string? BankAccountEncrypted { get; private set; }

    [Column("created_at")]
    [Precision(0)]
    public DateTimeOffset CreatedAt { get; private set; }

    [Column("updated_at")]
    [Precision(0)]
    public DateTimeOffset UpdatedAt { get; private set; }

    [ForeignKey("ResidentId")]
    [InverseProperty("ResidentSensitiveInfo")]
    public virtual Resident Resident { get; private set; } = null!;
}
