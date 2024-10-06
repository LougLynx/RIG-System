using System;
using System.Collections.Generic;

namespace Manage_Receive_Issues_Goods.Models;

public partial class Schedulereceived
{
    public int ScheduleId { get; set; }

    public int SupplierId { get; set; }

    public int DeliveryTimeId { get; set; }

    public int WeekdayId { get; set; }

    public int LeadTime { get; set; }

    public virtual ICollection<Actualreceived> Actualreceiveds { get; set; } = new List<Actualreceived>();

    public virtual Time DeliveryTime { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual Weekday Weekday { get; set; } = null!;
}
