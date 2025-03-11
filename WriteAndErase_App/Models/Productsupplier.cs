using System;
using System.Collections.Generic;

namespace WriteAndErase_App.Models;

public partial class Productsupplier
{
    public int Productsupplierid { get; set; }

    public string Productarticlenumber { get; set; } = null!;

    public int Supplierid { get; set; }

    public virtual Product ProductarticlenumberNavigation { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
