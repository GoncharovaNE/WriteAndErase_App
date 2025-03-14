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

        public double TotalOrderDiscount
        {
            get => Orderproducts != null
                ? Math.Round(Orderproducts.Sum(op =>
                    (op.ProductarticlenumberNavigation?.Productcost ?? 0) * op.Productquantity *
                    (op.ProductarticlenumberNavigation?.Productdiscountamount ?? 0) / 100), 2)
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
                    return "#ffffff"; // если заказ пуст

                bool allInStock = Orderproducts.All(op =>
                    op.ProductarticlenumberNavigation?.Productquantityinstock >= 3);

                bool anyOutOfStock = Orderproducts.Any(op =>
                    op.ProductarticlenumberNavigation?.Productquantityinstock == 0);

                if (anyOutOfStock)
                    return "#ff8c00"; // если есть отсутствующие товары

                if (allInStock)
                    return "#20b2aa"; // если все товары в наличии > 3

                return "#ffffff"; // по умолчанию
            }

            /*public string OrderStatus
            {
                get
                {
                    return Order?.OrderstatusNavigation?.Statusname ?? "Неизвестен";
                }
            }

            public string ProductInOrder
            {
                get
                {
                    return Order?.Orderproducts != null && Order.Orderproducts.Any()
                    ? string.Join(", ", Order.Orderproducts
                    .Where(op => op.ProductarticlenumberNavigation != null)
                    .Select(op => op.ProductarticlenumberNavigation.Productname))
                : "Отсутствует";
                }
            }

            public double TotalOrderSum
            {
                get
                {
                    return Order?.Orderproducts != null
                        ? Order.Orderproducts.Sum(op =>
                            (op.ProductarticlenumberNavigation?.Productcost ?? 0) * op.Productquantity)
                        : 0;
                }
            }

            public double TotalOrderDiscount
            {
                get
                {
                    return Order?.Orderproducts != null
                    ? Math.Round(Order.Orderproducts.Sum(op =>
                        (op.ProductarticlenumberNavigation?.Productcost ?? 0) * op.Productquantity *
                        (op.ProductarticlenumberNavigation?.Productdiscountamount ?? 0) / 100), 2)
                    : 0;
                }
            }

            public string ClientFullName
            {
                get
                {
                    return Order?.Orderclient != null && Order?.Orderclient != 1 && Order.OrderclientNavigation != null
                        ? $"{Order.OrderclientNavigation.Username} {Order.OrderclientNavigation.Usersurname} {Order.OrderclientNavigation.Userpatronymic}"
                        : "Гость";
                }
            }

            public string RowColor
            {
                get
                {
                    if (Order?.Orderproducts == null || !Order.Orderproducts.Any())
                        return "#ffffff"; // белый цвет, если заказ пуст или нет данных

                    bool allInStock = Order.Orderproducts.All(op =>
                        op.ProductarticlenumberNavigation?.Productquantityinstock >= 3);

                    bool anyOutOfStock = Order.Orderproducts.Any(op =>
                        op.ProductarticlenumberNavigation?.Productquantityinstock == 0);

                    if (anyOutOfStock)
                        return "#ff8c00"; // если хотя бы один товар отсутствует

                    if (allInStock)
                        return "#20b2aa"; // если все товары в наличии > 3

                    return "#ffffff"; // на случай, если не подходит ни под одно условие
                }
            }*/
        }
    }
}
