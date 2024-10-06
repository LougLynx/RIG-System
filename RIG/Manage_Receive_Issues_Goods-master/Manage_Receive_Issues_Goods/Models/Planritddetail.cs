using System;
using System.Collections.Generic;

namespace Manage_Receive_Issues_Goods.Models;

public partial class Planritddetail
{
    public int PlanDetailId { get; set; }

    public int PlanId { get; set; }

    public TimeOnly PlanTime { get; set; }

    public string PlanDetailName { get; set; } = null!;

    public virtual ICollection<Actualsritd> Actualsritds { get; set; } = new List<Actualsritd>();

    public virtual Planritd Plan { get; set; } = null!;

    public virtual ICollection<Status> Statuses { get; set; } = new List<Status>();
}
