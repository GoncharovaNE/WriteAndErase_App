using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
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

        private User? _currentUser;

        public User? CurrentUser { get => _currentUser; set => this.RaiseAndSetIfChanged(ref _currentUser, value); }

        private string _user;

        public string User { get => _user; set => this.RaiseAndSetIfChanged(ref _user, value); }

        private bool _IsVisibleBTCurrentOrder = false;

        public bool IsVisibleBTCurrentOrder { get => _IsVisibleBTCurrentOrder; set => this.RaiseAndSetIfChanged(ref _IsVisibleBTCurrentOrder, value); }

        private bool _IsCurrentOrder;

        public bool IsCurrentOrder { get => _IsCurrentOrder; set => this.RaiseAndSetIfChanged(ref _IsCurrentOrder, value); }
        
        private Order _currentOrder;

        public Order CurrentOrder { get => _currentOrder; set => this.RaiseAndSetIfChanged(ref _currentOrder, value); }

        #endregion

        public void ToBackProduct()
        {
            MainWindowViewModel.Instance.ContentPage = new ProductPage(CurrentUser.Userid, _currentOrder, IsVisibleBTCurrentOrder, IsCurrentOrder);
        }

        public CurrentOrderVM()
        {

        }

        public CurrentOrderVM(User CurrentUser, Order newOrder, bool IsVisibleBTCurrentOrder, bool IsCurrentOrder)
        {
             _listCurrentOrderProducts = newOrder.Orderproducts
            .Select(op => new Orderproduct
            {
                Productarticlenumber = op.Productarticlenumber,
                Productquantity = op.Productquantity,
                Order = newOrder,
                ProductarticlenumberNavigation = MainWindowViewModel.myСonnection.Products
                    .Include(p => p.ProductunitofmeasurementNavigation)
                    .FirstOrDefault(p => p.Productarticlenumber == op.Productarticlenumber)
            })
            .ToList();

            _currentOrder = newOrder;

            _currentUser = CurrentUser;

            OrderDate = newOrder.Orderdate.ToString("dd.MM.yyyy");

            OrderDateDelivery = newOrder.Orderdeliverydate.ToString("dd.MM.yyyy");

            OrderCode = newOrder.Ordercodetoreceive;

            PickupPoint = newOrder.OrderpickuppointNavigation.Pickuppointname;

            OrderSum = _listCurrentOrderProducts.Sum(x => x.ProductarticlenumberNavigation.Productcost * x.Productquantity);

            OrderDiscount = Math.Round((double)_listCurrentOrderProducts.Sum(x => (x.ProductarticlenumberNavigation.Productcost * x.Productquantity) * 
            (x.ProductarticlenumberNavigation.Productdiscountamount / 100)), 2);

            FinalOrderSum = Math.Max((double)(OrderSum - OrderDiscount), 0);

            bool availableProducts = _listCurrentOrderProducts.Any(x => x.ProductarticlenumberNavigation.Productquantityinstock < 3);
            DeliveryTime = availableProducts == true ? "6 дней" : "3 дня";

            _user = _currentUser?.Userid == 1 ? "Гость" : $"{_currentUser?.Username} {_currentUser?.Usersurname} {_currentUser?.Userpatronymic}";

            _IsVisibleBTCurrentOrder = IsVisibleBTCurrentOrder;
            _IsCurrentOrder = IsCurrentOrder;
        }

        public void SaveOrderToDatabase()
        {
            try
            {
                if (CurrentOrder.Orderproducts.Any())
                {
                    if (CurrentOrder.Orderid == 0)
                    {
                        MainWindowViewModel.myСonnection.Orders.Add(CurrentOrder);
                    }

                    foreach (Orderproduct? product in CurrentOrder.Orderproducts)
                    {
                        MainWindowViewModel.myСonnection.Orderproducts.Add(product);
                    }

                    MainWindowViewModel.myСonnection.SaveChanges();

                    MessageBoxManager.GetMessageBoxStandard("Успех!", "Заказ оформлен!", ButtonEnum.Ok).ShowAsync();

                    IsVisibleBTCurrentOrder = false;
                    IsCurrentOrder = false;

                    ClearNewOrder();

                    MainWindowViewModel.Instance.ContentPage = new ProductPage(CurrentUser.Userid, CurrentOrder, IsVisibleBTCurrentOrder, IsCurrentOrder);                    
                }
                else
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка!", "Нельзя сохранить пустой заказ!", ButtonEnum.Ok).ShowAsync();
                }
            }
            catch (DbUpdateException dbEx)
            {
                MessageBoxManager.GetMessageBoxStandard("Ошибка!", dbEx.InnerException?.Message ?? dbEx.Message, ButtonEnum.Ok).ShowAsync();
            }
        }

        private void ClearNewOrder()
        {
            CurrentOrder = new Order();
            ListCurrentOrderProducts = new List<Orderproduct>();

            OrderDate = string.Empty;
            OrderDateDelivery = string.Empty;
            OrderCode = 0;
            OrderSum = 0;
            OrderDiscount = 0;
            FinalOrderSum = 0;
            PickupPoint = string.Empty;
            DeliveryTime = string.Empty;
        }
    }
}
