using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Table("rooms")]
[Index("FacilityId", Name = "idx_rooms_facility_id")]
[Index("FacilityId", "RoomNumber", Name = "uq_room_per_facility", IsUnique = true)]
public partial class Room
{
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("room_number")]
    [StringLength(20)]
    public string RoomNumber { get; private set; } = null!;

    [Column("room_type")]
    [StringLength(50)]
    public string RoomType { get; private set; } = null!;

    [Column("facility_id")]
    public long FacilityId { get; private set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; private set; }

    [InverseProperty("Room")]
    public virtual ICollection<Bed> Beds { get; private set; } = new List<Bed>();

    [ForeignKey("FacilityId")]
    [InverseProperty("Rooms")]
    public virtual Facility Facility { get; private set; } = null!;
}
