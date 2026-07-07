using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NursingHome.Infrastructure.Persistence.Generated;

[Index("facility_id", Name = "idx_rooms_facility_id")]
[Index("facility_id", "room_number", Name = "uq_room_per_facility", IsUnique = true)]
public partial class room
{
    [Key]
    public Guid id { get; private set; }

    [StringLength(20)]
    public string room_number { get; private set; } = null!;

    [StringLength(50)]
    public string room_type { get; private set; } = null!;

    public Guid facility_id { get; private set; }

    public bool is_deleted { get; private set; }

    [InverseProperty("room")]
    public virtual ICollection<bed> beds { get; private set; } = new List<bed>();

    [ForeignKey("facility_id")]
    [InverseProperty("rooms")]
    public virtual facility facility { get; private set; } = null!;
}
