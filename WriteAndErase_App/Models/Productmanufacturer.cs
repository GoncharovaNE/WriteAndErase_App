using System;
using System.Collections.Generic;

namespace WriteAndErase_App.Models;

public partial class Productmanufacturer
{
    public int Productmanufacturerid { get; set; }

    public string Productarticlenumber { get; set; } = null!;

    public int Manufacturerid { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual Product ProductarticlenumberNavigation { get; set; } = null!;
}
