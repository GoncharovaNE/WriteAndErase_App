using System;
using System.Collections.Generic;

namespace WriteAndErase_App.Models;

public partial class Supplier
{
    public int Supplierid { get; set; }

    public string Suppliername { get; set; } = null!;

    public virtual ICollection<Productsupplier> Productsuppliers { get; set; } = new List<Productsupplier>();
}
