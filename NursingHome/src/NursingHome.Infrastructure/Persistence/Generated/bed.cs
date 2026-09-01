using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("beds")]
[Index("RoomId", Name = "idx_beds_room_id")]
[Index("Status", Name = "idx_beds_status")]
[Index("RoomId", "BedNumber", Name = "uq_bed_per_room", IsUnique = true)]
public partial class Bed
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("bed_number")]
    [StringLength(20)]
    public string BedNumber { get; private set; } = null!;

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; private set; } = null!;

    [Column("room_id")]
    public long RoomId { get; private set; }

    [InverseProperty("Bed")]
    public virtual ICollection<Resident> Residents { get; private set; } = new List<Resident>();

    [ForeignKey("RoomId")]
    [InverseProperty("Beds")]
    public virtual Room Room { get; private set; } = null!;
}
