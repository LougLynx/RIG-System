using System;
using System.Collections.Generic;

namespace Manage_Receive_Issues_Goods.Models;

public partial class Planritd
{
    public int PlanId { get; set; }

    public string PlanName { get; set; } = null!;

    public string PlanType { get; set; } = null!;

    public int TotalShipment { get; set; }

    public DateOnly EffectiveDate { get; set; }

    public virtual ICollection<Planritddetail> Planritddetails { get; set; } = new List<Planritddetail>();
}
