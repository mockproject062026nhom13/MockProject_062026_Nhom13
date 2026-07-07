using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("resident_id", Name = "idx_pre_admission_screenings_resident_id")]
public partial class pre_admission_screening
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(20)]
    [Unicode(false)]
    public string status { get; private set; } = null!;

    public Guid resident_id { get; private set; }

    public Guid screened_by { get; private set; }

    [Precision(0)]
    public DateTimeOffset created_at { get; private set; }

    [ForeignKey("resident_id")]
    [InverseProperty("pre_admission_screenings")]
    public virtual resident resident { get; private set; } = null!;

    [ForeignKey("screened_by")]
    [InverseProperty("pre_admission_screenings")]
    public virtual user screened_byNavigation { get; private set; } = null!;
}
