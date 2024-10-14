using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Models;

[Table("weekday")]
public partial class Weekday
{
    [Key]
    [Column("WeekdayID")]
    public int WeekdayId { get; set; }

    [StringLength(50)]
    public string DayName { get; set; } = null!;

    [InverseProperty("Weekday")]
    public virtual ICollection<Schedulereceived> Schedulereceiveds { get; set; } = new List<Schedulereceived>();
}
