using System;
using System.Collections.Generic;

namespace WriteAndErase_App.Models;

public partial class Status
{
    public int Statusid { get; set; }

    public string Statusname { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
