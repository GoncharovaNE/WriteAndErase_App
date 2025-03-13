using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteAndErase_App.Models;

namespace WriteAndErase_App.ViewModels
{
    class CurrentOrderVM : ViewModelBase
    {
        private List<Orderproduct> _listCurrentOrderProducts;

        public List<Orderproduct> ListCurrentOrderProducts { get => _listCurrentOrderProducts; set => this.RaiseAndSetIfChanged(ref _listCurrentOrderProducts, value); }

        private string _orderDate;
        public string OrderDate { get => _orderDate; set => this.RaiseAndSetIfChanged(ref _orderDate, value); }

        private string _orderDateDelivery;
        public string OrderDateDelivery { get => _orderDateDelivery; set => this.RaiseAndSetIfChanged(ref _orderDateDelivery, value); }

        private int _orderCode;
        public int OrderCode { get => _orderCode; set => this.RaiseAndSetIfChanged(ref _orderCode, value); }

        private float _orderSum;
        public float OrderSum { get => _orderSum; set => this.RaiseAndSetIfChanged(ref _orderSum, value); }

        private double _orderDiscount;
        public double OrderDiscount { get => _orderDiscount; set => this.RaiseAndSetIfChanged(ref _orderDiscount, value); }

        private double _finalOrderSum;
        public double FinalOrderSum { get => _finalOrderSum; set => this.RaiseAndSetIfChanged(ref _finalOrderSum, value); }

        private string _pickupPoint;
        public string PickupPoint { get => _pickupPoint; set => this.RaiseAndSetIfChanged(ref _pickupPoint, value); }

        private string _deliveryTime;
        public string DeliveryTime { get => _deliveryTime; set => this.RaiseAndSetIfChanged(ref _deliveryTime, value); }

        #region передача аргументов для возврата на страницу со списком товаров

        private string _user;

        public string User { get => _user; set => this.RaiseAndSetIfChanged(ref _user, value); }

        private bool _IsVisibleBTCurrentOrder = false;

        public bool IsVisibleBTCurrentOrder { get => _IsVisibleBTCurrentOrder; set => this.RaiseAndSetIfChanged(ref _IsVisibleBTCurrentOrder, value); }
        
        private bool _IsEnablePickuppoint = true;

        public bool IsEnablePickuppoint { get => _IsEnablePickuppoint; set => this.RaiseAndSetIfChanged(ref _IsEnablePickuppoint, value); }

        private bool _IsCurrentOrder;

        public bool IsCurrentOrder { get => _IsCurrentOrder; set => this.RaiseAndSetIfChanged(ref _IsCurrentOrder, value); }
        
        private Order _currentOrder;

        public Order CurrentOrder { get => _currentOrder; set => this.RaiseAndSetIfChanged(ref _currentOrder, value); }

        #endregion

        public void ToBackProduct()
        {
            User? userId = _listCurrentOrderProducts
                    .Select(x => x.Order.OrderclientNavigation)
                    .FirstOrDefault();

            MainWindowViewModel.Instance.ContentPage = new ProductPage(userId.Userid, _currentOrder, IsVisibleBTCurrentOrder, IsEnablePickuppoint, IsCurrentOrder);
        }

        public CurrentOrderVM()
        {

        }

        public CurrentOrderVM(Order currentOrder, bool IsVisibleBTCurrentOrder, bool IsEnablePickuppoint, bool IsCurrentOrder)
        {

            _listCurrentOrderProducts = MainWindowViewModel.myСonnection.Orderproducts
                                                                         .Where(x => x.Orderid == currentOrder.Orderid)
                                                                         .Include(x => x.Order)
                                                                         .Include(x => x.ProductarticlenumberNavigation)
                                                                         .ThenInclude(x => x.ProductunitofmeasurementNavigation)
                                                                         .ToList();

            _currentOrder = currentOrder;

            OrderDate = currentOrder.Orderdate.ToString("dd.MM.yyyy");

            OrderDateDelivery = currentOrder.Orderdeliverydate.ToString("dd.MM.yyyy");

            OrderCode = currentOrder.Ordercodetoreceive;

            PickupPoint = currentOrder.OrderpickuppointNavigation.Pickuppointname;

            OrderSum = _listCurrentOrderProducts.Sum(x => x.ProductarticlenumberNavigation.Productcost * x.Productquantity);

            OrderDiscount = Math.Round((double)_listCurrentOrderProducts.Sum(x => (x.ProductarticlenumberNavigation.Productcost * x.Productquantity) * 
            (x.ProductarticlenumberNavigation.Productdiscountamount / 100)), 2);

            FinalOrderSum = Math.Max((double)(OrderSum - OrderDiscount), 0);

            bool availableProducts = _listCurrentOrderProducts.Any(x => x.ProductarticlenumberNavigation.Productquantityinstock < 3);
            DeliveryTime = availableProducts == true ? "6 дней" : "3 дня";

            User? userId = _listCurrentOrderProducts
                   .Select(x => x.Order.OrderclientNavigation)
                   .FirstOrDefault();

            _user = userId?.Userid == 1 ? "Гость" : $"{userId?.Username} {userId?.Usersurname} {userId?.Userpatronymic}";

            _IsVisibleBTCurrentOrder = IsVisibleBTCurrentOrder;
            _IsEnablePickuppoint = IsEnablePickuppoint;
            _IsCurrentOrder = IsCurrentOrder;
        }        
    }
}
