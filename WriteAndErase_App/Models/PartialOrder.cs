using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteAndErase_App.Models
{
    partial class Order
    {
        public string ProductInOrder
        {
            get => Orderproducts != null && Orderproducts.Any()
                ? string.Join(", ", Orderproducts
                    .Where(op => op.ProductarticlenumberNavigation != null)
                    .Select(op => op.ProductarticlenumberNavigation.Productname))
                : "Отсутствует";
        }

        public double TotalOrderSum
        {
            get => Orderproducts != null
                ? Orderproducts.Sum(op =>
                    (op.ProductarticlenumberNavigation?.Productcost ?? 0) * op.Productquantity)
                : 0;
        }

        public double TotalOrderSumWithDiscount
        {
            get => Orderproducts != null
                ? Math.Round(Orderproducts.Sum(op =>
                    (op.ProductarticlenumberNavigation?.Productcost ?? 0) * op.Productquantity) - Orderproducts.Sum(op =>
                    (op.ProductarticlenumberNavigation?.Productcost ?? 0) * op.Productquantity) * (TotalOrderDiscountInPercent / 100),2)
                : 0;
        }

        public double TotalOrderDiscount
        {
            get => Orderproducts != null
                ? Math.Round(Orderproducts.Sum(op =>
                    (op.ProductarticlenumberNavigation?.Productcost ?? 0) * op.Productquantity) * (TotalOrderDiscountInPercent / 100), 2)
                : 0;
        }

        public double TotalOrderDiscountInPercent
        {
            get => Orderproducts != null
                ? Math.Round(Orderproducts.Sum(op =>
                    (op.ProductarticlenumberNavigation?.Productdiscountamount ?? 0)),2)
                : 0;
        }

        public string ClientFullName
        {
            get => Orderclient != null && Orderclient != 1 && OrderclientNavigation != null
                ? $"{OrderclientNavigation.Username} {OrderclientNavigation.Usersurname} {OrderclientNavigation.Userpatronymic}"
                : "Гость";
        }

        public string RowColor
        {
            get
            {
                if (Orderproducts == null || !Orderproducts.Any())
                    return "#ffffff"; 

                bool allInStock = Orderproducts.All(op =>
                    op.ProductarticlenumberNavigation?.Productquantityinstock >= 3);

                bool anyOutOfStock = Orderproducts.Any(op =>
                    op.ProductarticlenumberNavigation?.Productquantityinstock == 0);

                if (anyOutOfStock)
                    return "#ff8c00"; 

                if (allInStock)
                    return "#20b2aa"; 

                return "#ffffff";
            }                       
        }
    }
}
