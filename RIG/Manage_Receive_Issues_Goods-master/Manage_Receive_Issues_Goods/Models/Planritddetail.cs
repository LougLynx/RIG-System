using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Models;

[Table("planritddetails")]
[Index("PlanId", Name = "PlanID")]
public partial class Planritddetail
{
    [Key]
    [Column("PlanDetailID")]
    public int PlanDetailId { get; set; }

    [Column("PlanID")]
    public int PlanId { get; set; }

    public DateOnly PlanDate { get; set; }

    [Column(TypeName = "time")]
    public TimeOnly PlanTime { get; set; }

    [StringLength(100)]
    public string PlanDetailName { get; set; } = null!;

    [InverseProperty("PlanDetail")]
    public virtual ICollection<Actualsritd> Actualsritds { get; set; } = new List<Actualsritd>();

    [ForeignKey("PlanId")]
    [InverseProperty("Planritddetails")]
    public virtual Planritd Plan { get; set; } = null!;

    [InverseProperty("PlanDetail")]
    public virtual ICollection<Status> Statuses { get; set; } = new List<Status>();
}
