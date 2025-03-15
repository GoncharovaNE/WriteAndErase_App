using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using WriteAndErase_App.Models;

namespace WriteAndErase_App.ViewModels
{
    class OrdersVM : ViewModelBase
    {
        private string _user;

        public string User { get => _user; set => this.RaiseAndSetIfChanged(ref _user, value); }

        private User? _currentUser;

        public User? CurrentUser { get => _currentUser; set => this.RaiseAndSetIfChanged(ref _currentUser, value); }

        private List<Order> _listOrderProduct;

        public List<Order> ListOrderProduct { get => _listOrderProduct; set => this.RaiseAndSetIfChanged(ref _listOrderProduct, value); }

        private Orderproduct _selectedOrder;

        public Orderproduct SelectedOrder
        {
            get => _selectedOrder;
            set => this.RaiseAndSetIfChanged(ref _selectedOrder, value);
        }

        public ReactiveCommand<int, Unit> EditOrderproductCommand { get; }

        

        public OrdersVM()
        {
            _listOrderProduct = MainWindowViewModel.myСonnection.Orders
                                .Include(o => o.Orderproducts)
                                .ThenInclude(op => op.ProductarticlenumberNavigation)
                                .Include(o => o.OrderstatusNavigation)
                                .Where(o => o.Orderproducts.Any())
                                .ToList();
        }

        public OrdersVM(User currentUser)
        {
            _currentUser = currentUser;
            _user = _currentUser?.Userid == 1 ? "Гость" : $"{_currentUser?.Username} {_currentUser?.Usersurname} {_currentUser?.Userpatronymic}";

            _listOrderProduct = MainWindowViewModel.myСonnection.Orders
                                .Include(o => o.Orderproducts)
                                .ThenInclude(op => op.ProductarticlenumberNavigation)
                                .Include(o => o.OrderstatusNavigation)
                                .Where(o => o.Orderproducts.Any()) 
                                .ToList();

            EditOrderproductCommand = ReactiveCommand.Create<int>(ToEditOrder);
        }

        public void ToBack()
        {
            MainWindowViewModel.Instance.ContentPage = new ProductPage(CurrentUser.Userid);
        }

        public void ToEditOrder(int id)
        {
            MainWindowViewModel.Instance.ContentPage = new EditOrderPage(id, CurrentUser);
        }

        #region Сортировка и фильтрация

        int _selectedSort = 0;
        public int SelectedSort { get => _selectedSort; set { _selectedSort = value; filtersOrders(); } }

        int _selectedFilter = 0;
        public int SelectedFilter { get => _selectedFilter; set { _selectedFilter = value; filtersOrders(); } }

        #endregion

        private void filtersOrders()
        {
            ListOrderProduct = MainWindowViewModel.myСonnection.Orders
                                .Include(o => o.Orderproducts)
                                .ThenInclude(op => op.ProductarticlenumberNavigation)
                                .Include(o => o.OrderstatusNavigation)
                                .Where(o => o.Orderproducts.Any())
                                .ToList();

            switch (_selectedSort)
            {
                case 0:
                    ListOrderProduct = ListOrderProduct.ToList();
                    break;
                case 1:
                    ListOrderProduct = ListOrderProduct.OrderBy(x => x.Orderproducts
                    .Sum(x => x.ProductarticlenumberNavigation.Productcost * x.Productquantity)).ToList();
                    break;
                case 2:
                    ListOrderProduct = ListOrderProduct.OrderByDescending(x => x.Orderproducts
                    .Sum(x => x.ProductarticlenumberNavigation.Productcost * x.Productquantity)).ToList();
                    break;
            }

            float dis1 = 10F;
            float dis2 = 11F;
            float dis3 = 14F;
            float dis4 = 15F;

            switch (_selectedFilter)
            {
                case 0:
                    ListOrderProduct = ListOrderProduct.ToList();
                    break;
                case 1:
                    ListOrderProduct = ListOrderProduct.Where(x => x.Orderproducts
                    .Sum(x => x.ProductarticlenumberNavigation.Productdiscountamount) <= dis1).ToList();
                    break;
                case 2:
                    ListOrderProduct = ListOrderProduct.Where(x => x.Orderproducts
                    .Sum(x => x.ProductarticlenumberNavigation.Productdiscountamount) <= dis3 && x.Orderproducts
                    .Sum(x => x.ProductarticlenumberNavigation.Productdiscountamount) >= dis2).ToList();
                    break;
                case 3:
                    ListOrderProduct = ListOrderProduct.Where(x => x.Orderproducts
                    .Sum(x => x.ProductarticlenumberNavigation.Productdiscountamount) >= dis4).ToList();
                    break;
            }
        }       
    }
}
