using System;
using System.Collections.Generic;

namespace WriteAndErase_App.Models;

public partial class Manufacturer
{
    public int Manufacturerid { get; set; }

    public string Manufacturername { get; set; } = null!;

    public virtual ICollection<Productmanufacturer> Productmanufacturers { get; set; } = new List<Productmanufacturer>();
}
