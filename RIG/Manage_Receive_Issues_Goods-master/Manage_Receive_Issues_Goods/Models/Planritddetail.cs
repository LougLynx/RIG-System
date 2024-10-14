using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Models;

[Table("planritddetails")]
[Index("PlanId", Name = "PlanID")]
[Index("StatusIssueId", Name = "StatusIssueID")]
[Index("StatusReceiveId", Name = "StatusReceiveID")]
public partial class Planritddetail
{
    [Key]
    [Column("PlanDetailID")]
    public int PlanDetailId { get; set; }

    [Column("PlanID")]
    public int PlanId { get; set; }

    [Column(TypeName = "time")]
    public TimeOnly PlanTime { get; set; }

    [StringLength(100)]
    public string PlanDetailName { get; set; } = null!;

    [Column("StatusReceiveID")]
    public int? StatusReceiveId { get; set; }

    [Column("StatusIssueID")]
    public int? StatusIssueId { get; set; }

    [InverseProperty("PlanDetail")]
    public virtual ICollection<Actualsissuestlip> Actualsissuestlips { get; set; } = new List<Actualsissuestlip>();

    [InverseProperty("PlanDetail")]
    public virtual ICollection<Actualsreceivedenso> Actualsreceivedensos { get; set; } = new List<Actualsreceivedenso>();

    [ForeignKey("PlanId")]
    [InverseProperty("Planritddetails")]
    public virtual Planritd Plan { get; set; } = null!;

    [ForeignKey("StatusIssueId")]
    [InverseProperty("PlanritddetailStatusIssues")]
    public virtual Statusesritd? StatusIssue { get; set; }

    [ForeignKey("StatusReceiveId")]
    [InverseProperty("PlanritddetailStatusReceives")]
    public virtual Statusesritd? StatusReceive { get; set; }
}
