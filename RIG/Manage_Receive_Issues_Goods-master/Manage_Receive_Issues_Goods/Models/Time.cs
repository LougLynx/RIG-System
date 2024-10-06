using System;
using System.Collections.Generic;

namespace Manage_Receive_Issues_Goods.Models;

public partial class Time
{
    public int TimeId { get; set; }

    public TimeOnly Time1 { get; set; }

    public virtual ICollection<Schedulereceived> Schedulereceiveds { get; set; } = new List<Schedulereceived>();
}
