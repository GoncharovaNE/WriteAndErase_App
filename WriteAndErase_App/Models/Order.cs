using System;
using System.Collections.Generic;

namespace WriteAndErase_App.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public int Orderstatus { get; set; }

    public DateOnly Orderdate { get; set; }

    public DateOnly? Orderdeliverydate { get; set; }

    public int? Orderpickuppoint { get; set; }

    public int Orderclient { get; set; }

    public int? Ordercodetoreceive { get; set; }

    public virtual User OrderclientNavigation { get; set; } = null!;

    public virtual Pickuppoint? OrderpickuppointNavigation { get; set; }

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();

    public virtual Status OrderstatusNavigation { get; set; } = null!;
}
