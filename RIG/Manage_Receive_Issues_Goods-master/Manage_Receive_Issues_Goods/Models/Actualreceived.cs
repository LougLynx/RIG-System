using System;
using System.Collections.Generic;

namespace Manage_Receive_Issues_Goods.Models;

public partial class Actualreceived
{
    public int ActualReceivedId { get; set; }

    public DateTime ActualDeliveryTime { get; set; }

    public int ActualLeadTime { get; set; }

    public int ScheduleId { get; set; }

    public virtual Schedulereceived Schedule { get; set; } = null!;
}
