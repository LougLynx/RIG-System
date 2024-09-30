using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Models;

[Table("actualreceived")]
[Index("ScheduleId", Name = "ScheduleID")]
public partial class Actualreceived
{
    [Key]
    [Column("ActualReceivedID")]
    public int ActualReceivedId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ActualDeliveryTime { get; set; }

    public int ActualLeadTime { get; set; }

    [Column("ScheduleID")]
    public int ScheduleId { get; set; }

    [ForeignKey("ScheduleId")]
    [InverseProperty("Actualreceiveds")]
    public virtual Schedulereceived Schedule { get; set; } = null!;
}
