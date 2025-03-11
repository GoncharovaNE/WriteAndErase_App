using System;
using System.Collections.Generic;

namespace WriteAndErase_App.Models;

public partial class Pickuppoint
{
    public int Pickuppointid { get; set; }

    public string Pickuppointname { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
