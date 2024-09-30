using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Models;

[Table("time")]
public partial class Time
{
    [Key]
    [Column("TimeID")]
    public int TimeId { get; set; }

    [Column("Time", TypeName = "time")]
    public TimeOnly Time1 { get; set; }

    [InverseProperty("DeliveryTime")]
    public virtual ICollection<Schedulereceived> Schedulereceiveds { get; set; } = new List<Schedulereceived>();
}
