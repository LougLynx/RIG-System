using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Models;

[Table("statuses")]
[Index("PlanDetailId", Name = "PlanDetailID")]
public partial class Status
{
    [Key]
    [Column("StatusID")]
    public int StatusId { get; set; }

    [Column("PlanDetailID")]
    public int PlanDetailId { get; set; }

    [Column("Status", TypeName = "enum('Pending','In Transit','Delivered')")]
    public string Status1 { get; set; } = null!;

    [ForeignKey("PlanDetailId")]
    [InverseProperty("Statuses")]
    public virtual Planritddetail PlanDetail { get; set; } = null!;
}
