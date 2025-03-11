using System;
using System.Collections.Generic;

namespace WriteAndErase_App.Models;

public partial class Unitofmeasurement
{
    public int Unitofmeasurementid { get; set; }

    public string Unitofmeasurementname { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
