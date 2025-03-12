using System;
using System.Collections.Generic;

namespace WriteAndErase_App.Models;

public partial class Product
{
    public string Productarticlenumber { get; set; } = null!;

    public string Productname { get; set; } = null!;

    public int Productunitofmeasurement { get; set; }

    public float Productcost { get; set; }

    public short? Productmaximumpossiblediscountamount { get; set; }

    public int Productcategory { get; set; }

    public float? Productdiscountamount { get; set; }

    public int Productquantityinstock { get; set; }

    public string Productdescription { get; set; } = null!;

    public string Productphoto { get; set; } = null!;

    public string? Productstatus { get; set; }

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();

    public virtual Category ProductcategoryNavigation { get; set; } = null!;

    public virtual ICollection<Productmanufacturer> Productmanufacturers { get; set; } = new List<Productmanufacturer>();

    public virtual ICollection<Productsupplier> Productsuppliers { get; set; } = new List<Productsupplier>();

    public virtual Unitofmeasurement ProductunitofmeasurementNavigation { get; set; } = null!;
}
