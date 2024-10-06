using System;
using System.Collections.Generic;

namespace Manage_Receive_Issues_Goods.Models;

public partial class Status
{
    public int StatusId { get; set; }

    public int PlanDetailId { get; set; }

    public string Status1 { get; set; } = null!;

    public virtual Planritddetail PlanDetail { get; set; } = null!;
}
