using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("room_id", Name = "idx_beds_room_id")]
[Index("status", Name = "idx_beds_status")]
[Index("room_id", "bed_number", Name = "uq_bed_per_room", IsUnique = true)]
public partial class bed
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(20)]
    public string bed_number { get; private set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string status { get; private set; } = null!;

    public Guid room_id { get; private set; }

    [InverseProperty("bed")]
    public virtual ICollection<resident> residents { get; private set; } = new List<resident>();

    [ForeignKey("room_id")]
    [InverseProperty("beds")]
    public virtual room room { get; private set; } = null!;
}
