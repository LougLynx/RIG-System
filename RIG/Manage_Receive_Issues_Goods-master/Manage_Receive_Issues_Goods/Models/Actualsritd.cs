using System;
using System.Collections.Generic;

namespace Manage_Receive_Issues_Goods.Models;

public partial class Actualsritd
{
    public int ActualId { get; set; }

    public int PlanDetailId { get; set; }

    public DateTime ActualTime { get; set; }

    public virtual Planritddetail PlanDetail { get; set; } = null!;
}
