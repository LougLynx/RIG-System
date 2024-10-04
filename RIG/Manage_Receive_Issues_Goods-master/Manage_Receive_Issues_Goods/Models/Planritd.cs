using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_Receive_Issues_Goods.Models;

[Table("planritd")]
public partial class Planritd
{
    [Key]
    [Column("PlanID")]
    public int PlanId { get; set; }

    [StringLength(100)]
    public string PlanName { get; set; } = null!;

    [Column(TypeName = "enum('Received','Issued')")]
    public string PlanType { get; set; } = null!;

    public int TotalShipment { get; set; }

    public DateOnly EffectiveDate { get; set; }

    //[InverseProperty("Plan")]
    //public virtual ICollection<Planritddetail> Planritddetails { get; set; } = new List<Planritddetail>();
}
