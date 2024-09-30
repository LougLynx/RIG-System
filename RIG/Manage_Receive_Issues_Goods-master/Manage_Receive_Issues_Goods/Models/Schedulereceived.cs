using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Models;

[Table("schedulereceived")]
[Index("DeliveryTimeId", Name = "DeliveryTimeID")]
[Index("SupplierId", Name = "SupplierID")]
[Index("WeekdayId", Name = "WeekdayID")]
public partial class Schedulereceived
{
    [Key]
    [Column("ScheduleID")]
    public int ScheduleId { get; set; }

    [Column("SupplierID")]
    public int SupplierId { get; set; }

    [Column("DeliveryTimeID")]
    public int DeliveryTimeId { get; set; }

    [Column("WeekdayID")]
    public int WeekdayId { get; set; }

    public int LeadTime { get; set; }

    [InverseProperty("Schedule")]
    public virtual ICollection<Actualreceived> Actualreceiveds { get; set; } = new List<Actualreceived>();

    [ForeignKey("DeliveryTimeId")]
    [InverseProperty("Schedulereceiveds")]
    public virtual Time DeliveryTime { get; set; } = null!;

    [ForeignKey("SupplierId")]
    [InverseProperty("Schedulereceiveds")]
    public virtual Supplier Supplier { get; set; } = null!;

    [ForeignKey("WeekdayId")]
    [InverseProperty("Schedulereceiveds")]
    public virtual Weekday Weekday { get; set; } = null!;
}
