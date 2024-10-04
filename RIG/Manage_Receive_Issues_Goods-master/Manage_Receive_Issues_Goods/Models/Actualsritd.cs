using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Models;

[Table("actualsritd")]
[Index("PlanDetailId", Name = "PlanDetailID")]
public partial class Actualsritd
{
    [Key]
    [Column("ActualID")]
    public int ActualId { get; set; }

    [Column("PlanDetailID")]
    public int PlanDetailId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ActualTime { get; set; }

    /*[ForeignKey("PlanDetailId")]
    [InverseProperty("Actualsritds")]
    public virtual Planritddetail PlanDetail { get; set; } = null!;*/
}
