using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteAndErase_App.Models
{
    partial class Product
    {
        public string ManufacturerNames
        {
            get
            {
                return Productmanufacturers != null && Productmanufacturers.Any()
                    ? string.Join(", ", Productmanufacturers.Select(pm => pm.Manufacturer.Manufacturername))
                    : "Отсутствует";
            }
        }

        public bool IsVisCostWithDis
        {
            get
            {
                bool isVisCostWithDis;
                if (Productdiscountamount != null && Productdiscountamount != 0)
                {
                    return isVisCostWithDis = true;
                }
                return isVisCostWithDis = false;
            }
        }

        public string DiscountIsOrNot
        {
            get
            {
                if (IsVisCostWithDis == true) return $"{Productdiscountamount}%"; 
                else return "Нет скидки";
            }
        }

        public string DiscountWithDis
        {
            get
            {
                if (IsVisCostWithDis == true)
                {
                    float? dif = (float)Productcost * (Productdiscountamount / 100);
                    float? cost = (float)Productcost - dif;
                    return $"{cost}₽";
                }
                else return $"{Productcost}₽";
            }
        }

        public string Color
        {
            get
            {
                string color;
                if (Productdiscountamount != null && Productdiscountamount != 0 && Productdiscountamount >= 14.99)
                {
                    return color = "#7fff00";
                }
                return "";
            }
        }
    }
}
