using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Models;

[Table("statusesritd")]
public partial class Statusesritd
{
    [Key]
    [Column("StatusID")]
    public int StatusId { get; set; }

    [Column(TypeName = "enum('Pending','In Transit','Delivered','Received')")]
    public string Status { get; set; } = null!;

    [InverseProperty("StatusIssue")]
    public virtual ICollection<Planritddetail> PlanritddetailStatusIssues { get; set; } = new List<Planritddetail>();

    [InverseProperty("StatusReceive")]
    public virtual ICollection<Planritddetail> PlanritddetailStatusReceives { get; set; } = new List<Planritddetail>();
}
