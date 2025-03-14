using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteAndErase_App.Models
{
    partial class Orderproduct
    {        
        public string ProductUnitOfMeasurement
        {
            get
            {
                return ProductarticlenumberNavigation.ProductunitofmeasurementNavigation.Unitofmeasurementname.ToString() ;
            }
        }

        public string ManufacturerNamesInOrderproduct
        {
            get
            {
                return ProductarticlenumberNavigation.Productmanufacturers != null && ProductarticlenumberNavigation.Productmanufacturers.Any()
                    ? string.Join(", ", ProductarticlenumberNavigation.Productmanufacturers.Select(pm => pm.Manufacturer.Manufacturername))
                    : "Отсутствует";
            }
        }               

        public bool IsVisCostWithDisInOrderproduct
        {
            get
            {
                bool isVisCostWithDis;
                if (ProductarticlenumberNavigation.Productdiscountamount != null && ProductarticlenumberNavigation.Productdiscountamount != 0)
                {
                    return isVisCostWithDis = true;
                }
                return isVisCostWithDis = false;
            }
        }

        public string DiscountIsOrNotInOrderproduct
        {
            get
            {
                if (IsVisCostWithDisInOrderproduct == true) return $"{ProductarticlenumberNavigation.Productdiscountamount}%";
                else return "Нет скидки";
            }
        }

        public string CostWithDisInOrderproduct
        {
            get
            {
                if (IsVisCostWithDisInOrderproduct == true)
                {
                    float? dif = (float)ProductarticlenumberNavigation.Productcost * (ProductarticlenumberNavigation.Productdiscountamount / 100);
                    float? cost = (float)ProductarticlenumberNavigation.Productcost - dif;
                    return $"{cost}₽";
                }
                else return $"{ProductarticlenumberNavigation.Productcost}₽";
            }
        }

        public string ColorInOrderproduct
        {
            get
            {
                string color;
                if (ProductarticlenumberNavigation.Productdiscountamount != null && 
                    ProductarticlenumberNavigation.Productdiscountamount != 0 && 
                    ProductarticlenumberNavigation.Productdiscountamount >= 14.99)
                {
                    return color = "#7fff00";
                }
                return "";
            }
        }
    }    
}
