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
    }
}
