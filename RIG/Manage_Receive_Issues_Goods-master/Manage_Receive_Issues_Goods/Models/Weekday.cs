using System;
using System.Collections.Generic;

namespace Manage_Receive_Issues_Goods.Models;

public partial class Weekday
{
    public int WeekdayId { get; set; }

    public string DayName { get; set; } = null!;

    public virtual ICollection<Schedulereceived> Schedulereceiveds { get; set; } = new List<Schedulereceived>();
}
