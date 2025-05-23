﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Models;

[Table("actualsissuetlip")]
[Index("PlanDetailId", Name = "PlanDetailID")]
[Index("UserId", Name = "UserId")]
public partial class Actualsissuetlip
{
    [Key]
    [Column("ActualID")]
    public int ActualId { get; set; }

    [Column("PlanDetailID")]
    public int? PlanDetailId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ActualTime { get; set; }

    [StringLength(450)]
    public string? UserId { get; set; }

    [ForeignKey("PlanDetailId")]
    [InverseProperty("Actualsissuetlips")]
    public virtual Planrdtddetail? PlanDetail { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Actualsissuetlips")]
    public virtual Aspnetuser? User { get; set; }
}
