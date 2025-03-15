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
    /// <summary>
    /// ViewModel для управления заказами.
    /// </summary>
    class OrdersVM : ViewModelBase
    {
        /// <summary>
        /// ФИО пользователя.
        /// </summary>
        private string _user;
        public string User { get => _user; set => this.RaiseAndSetIfChanged(ref _user, value); }

        /// <summary>
        /// Текущий пользователь.
        /// </summary>
        private User? _currentUser;
        public User? CurrentUser { get => _currentUser; set => this.RaiseAndSetIfChanged(ref _currentUser, value); }

        /// <summary>
        /// Список заказов пользователя.
        /// </summary>
        private List<Order> _listOrderProduct;
        public List<Order> ListOrderProduct { get => _listOrderProduct; set => this.RaiseAndSetIfChanged(ref _listOrderProduct, value); }

        /// <summary>
        /// Выбранный заказ.
        /// </summary>
        private Orderproduct _selectedOrder;
        public Orderproduct SelectedOrder { get => _selectedOrder; set => this.RaiseAndSetIfChanged(ref _selectedOrder, value); }

        /// <summary>
        /// Команда для редактирования выбранного заказа.
        /// </summary>
        public ReactiveCommand<int, Unit> EditOrderproductCommand { get; }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public OrdersVM()
        {
            _listOrderProduct = MainWindowViewModel.myСonnection.Orders
                                .Include(o => o.Orderproducts)
                                .ThenInclude(op => op.ProductarticlenumberNavigation)
                                .Include(o => o.OrderstatusNavigation)
                                .Where(o => o.Orderproducts.Any())
                                .ToList();
        }

        /// <summary>
        /// Конструктор с параметром пользователя.
        /// </summary>
        /// <param name="currentUser">Текущий пользователь.</param>
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

        /// <summary>
        /// Метод для возврата на страницу товаров.
        /// </summary>
        public void ToBack()
        {
            MainWindowViewModel.Instance.ContentPage = new ProductPage(CurrentUser.Userid);
        }

        /// <summary>
        /// Метод для перехода на страницу редактирования заказа.
        /// </summary>
        /// <param name="id">Идентификатор заказа.
        /// <param name="СurrentUser">Текущий пользователь.</param>
        public void ToEditOrder(int id)
        {
            MainWindowViewModel.Instance.ContentPage = new EditOrderPage(id, CurrentUser);
        }

        #region Сортировка и фильтрация заказов

        /// <summary>
        /// Выбранный параметр сортировки (0 - без сортировки, 1 - по возрастанию суммы, 2 - по убыванию суммы).
        /// </summary>
        int _selectedSort = 0;
        public int SelectedSort { get => _selectedSort; set { _selectedSort = value; filtersOrders(); } }

        /// <summary>
        /// Выбранный параметр фильтрации скидок.
        /// </summary>
        int _selectedFilter = 0;
        public int SelectedFilter { get => _selectedFilter; set { _selectedFilter = value; filtersOrders(); } }

        /// <summary>
        /// Метод для сортировки и фильтрации списка заказов.
        /// </summary>
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

        #endregion
    }
}
