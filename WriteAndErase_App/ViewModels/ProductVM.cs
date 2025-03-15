using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteAndErase_App.Models;

namespace WriteAndErase_App.ViewModels
{
    class ProductVM : ViewModelBase
    {
        #region Свойства

        private string _user;

        public string User { get => _user; set => this.RaiseAndSetIfChanged(ref _user, value); }

        private User? _currentUser;

        public User? CurrentUser { get => _currentUser; set => this.RaiseAndSetIfChanged(ref _currentUser, value); }

        private List<User> _listUser;

        public List<User> ListUser { get => _listUser; set => this.RaiseAndSetIfChanged(ref _listUser, value); }

        private List<Product> _listProduct;        

        public List<Product> ListProduct { get => _listProduct; set => this.RaiseAndSetIfChanged(ref _listProduct, value); }

        private List<Pickuppoint> _listPickupPoint;

        public List<Pickuppoint> ListPickupPoint { get => _listPickupPoint; set => this.RaiseAndSetIfChanged(ref _listPickupPoint, value); }

        private bool _isForAdminMeneg = false;

        public bool IsForAdminMeneg { get => _isForAdminMeneg; set => this.RaiseAndSetIfChanged(ref _isForAdminMeneg, value); }

        #endregion

        public ProductVM()
        {            
            _listProduct = MainWindowViewModel.myСonnection.Products.AsNoTracking().Include(x => x.Productmanufacturers).ThenInclude(x => x.Manufacturer).ToList();
            _listPickupPoint = MainWindowViewModel.myСonnection.Pickuppoints.ToList();
        }

        public ProductVM(int id)
        {
            _listUser = MainWindowViewModel.myСonnection.Users.Include(x => x.UserroleNavigation).ToList();
            _currentUser = _listUser.FirstOrDefault(x => x.Userid == id);
            _user = _currentUser?.Userid == 1 ? "Гость" : $"{_currentUser?.Username} {_currentUser?.Usersurname} {_currentUser?.Userpatronymic}";

            _listOrder = MainWindowViewModel.myСonnection.Orders.ToList();
            _listPickupPoint = MainWindowViewModel.myСonnection.Pickuppoints.ToList();

            _listProduct = MainWindowViewModel.myСonnection.Products.AsNoTracking().Include(x => x.Productmanufacturers).ThenInclude(x => x.Manufacturer).ToList();

            IsForAdminMeneg = _currentUser.Userrole == 2 || _currentUser.Userrole == 3 ? true : false;
        }

        public ProductVM(int id, Order currentOrder, bool IsVisibleBTCurrentOrder, bool IsCurrentOrder, bool IsAdminMeneg)
        {
            _listUser = MainWindowViewModel.myСonnection.Users.Include(x => x.UserroleNavigation).ToList();
            _currentUser = _listUser.FirstOrDefault(x => x.Userid == id);
            _user = _currentUser?.Userid == 1 ? "Гость" : $"{_currentUser?.Username} {_currentUser?.Usersurname} {_currentUser?.Userpatronymic}";

            _listOrder = MainWindowViewModel.myСonnection.Orders.ToList();
            _listPickupPoint = MainWindowViewModel.myСonnection.Pickuppoints.ToList();

            _listProduct = MainWindowViewModel.myСonnection.Products.AsNoTracking().Include(x => x.Productmanufacturers).ThenInclude(x => x.Manufacturer).ToList();

            NewOrder = currentOrder;
            _IsVisibleBTCurrentOrder = IsVisibleBTCurrentOrder;
            _IsCurrentOrder = IsCurrentOrder;

            IsForAdminMeneg = _currentUser.Userrole == 2 || _currentUser.Userrole == 3 ? true : false;

            IsForAdminMeneg = IsAdminMeneg;
        }

        public void ToBackAuth()
        {
            MainWindowViewModel.Instance.ContentPage = new AuthorizationPage();
        }

        public void ToCurrentOrder()
        {
            MainWindowViewModel.Instance.ContentPage = new CurrentOrderView(CurrentUser, NewOrder, IsVisibleBTCurrentOrder, IsCurrentOrder, IsForAdminMeneg);
        }

        public void ToOrderPage()
        {
            MainWindowViewModel.Instance.ContentPage = new OrdersPage(CurrentUser);
        }

        #region Сортировка, поиск и фильтрация

        int _countItemsList = MainWindowViewModel.myСonnection.Products.Include(x => x.Productmanufacturers).ThenInclude(x => x.Manufacturer).ToList().Count;

        public int CountItemsList { get => _countItemsList; set => this.RaiseAndSetIfChanged(ref _countItemsList, value); }

        int _countItemsDB = MainWindowViewModel.myСonnection.Products.Include(x => x.Productmanufacturers).ThenInclude(x => x.Manufacturer).ToList().Count;

        public int CountItemsDB { get => _countItemsDB; set => this.RaiseAndSetIfChanged(ref _countItemsDB, value); }

        string _search;
        public string Search { get => _search; set { _search = this.RaiseAndSetIfChanged(ref _search, value); filtersProduct(); } }

        int _selectedSort = 0;
        public int SelectedSort { get => _selectedSort; set { _selectedSort = value; filtersProduct(); } }

        int _selectedFilter = 0;
        public int SelectedFilter { get => _selectedFilter; set { _selectedFilter = value; filtersProduct(); } }

        private bool _noResults;
        public bool NoResults
        {
            get => _noResults;
            set => this.RaiseAndSetIfChanged(ref _noResults, value);
        }      

        public void filtersProduct()
        {
            ListProduct = MainWindowViewModel.myСonnection.Products.Include(x => x.Productmanufacturers).ThenInclude(x => x.Manufacturer).ToList();

            if (!string.IsNullOrEmpty(_search))
            {
                ListProduct = ListProduct.Where(x => x.Productname.ToLower().Contains(_search.ToLower())).ToList();
                CountItemsList = ListProduct.Count;
            }                      

            switch (_selectedSort)
            {
                case 0:
                    ListProduct = ListProduct.ToList();
                    CountItemsList = ListProduct.Count;
                    break;
                case 1:
                    ListProduct = ListProduct.OrderBy(x => x.Productcost).ToList();
                    CountItemsList = ListProduct.Count;
                    break;
                case 2:
                    ListProduct = ListProduct.OrderByDescending(x => x.Productcost).ToList();
                    CountItemsList = ListProduct.Count;
                    break;
            }

            float dis1 = 9.99F;
            float dis2 = 10F;
            float dis3 = 14.99F;
            float dis4 = 15F;

            switch (_selectedFilter)
            {
                case 0:
                    ListProduct = ListProduct.ToList();
                    CountItemsList = ListProduct.Count;
                    break;
                case 1:
                    ListProduct = ListProduct.Where(x => x.Productdiscountamount <= dis1).ToList();
                    CountItemsList = ListProduct.Count;
                    break;
                case 2:
                    ListProduct = ListProduct.Where(x => x.Productdiscountamount >= dis2 && x.Productdiscountamount <= dis3).ToList();
                    CountItemsList = ListProduct.Count;
                    break;
                case 3:
                    ListProduct = ListProduct.Where(x => x.Productdiscountamount >= dis4).ToList();
                    CountItemsList = ListProduct.Count;
                    break;              
            }

            if (CountItemsList == 0) NoResults = true;
            else NoResults = false;
        }

        #endregion

        #region Формирование заказа

        private bool _IsCurrentOrder;

        public bool IsCurrentOrder { get => _IsCurrentOrder; set => this.RaiseAndSetIfChanged(ref _IsCurrentOrder, value); }

        private List<Order> _listOrder;               

        public List<Order> ListOrder { get => _listOrder; set => this.RaiseAndSetIfChanged(ref _listOrder, value); }

        private Order? _newOrder = new Order();

        public Order? NewOrder { get => _newOrder; set => this.RaiseAndSetIfChanged(ref _newOrder, value); }

        private bool _IsVisibleBTCurrentOrder = false;

        public bool IsVisibleBTCurrentOrder { get => _IsVisibleBTCurrentOrder; set => this.RaiseAndSetIfChanged(ref _IsVisibleBTCurrentOrder, value); }
        
        public int GenerateUniqueOrderCode()
        {
            Random rnd = new Random();
            int newCode;
            bool codeExists;

            do
            {
                newCode = rnd.Next(100, 999);
                codeExists = _listOrder.Any(x => x.Ordercodetoreceive == newCode);
            }
            while (codeExists);

            return newCode;
        }

        public async void AddToOrder(Product product)
        {
            try
            {                
                string Messege;                

                if (_IsCurrentOrder == false)
                {
                    try
                    {   
                        if (NewOrder.OrderpickuppointNavigation == null)
                        {
                            Messege = "Выберите пункт выдачи! Заказ не создан!";
                            ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Внимание! Ошибка пункта выдачи!",
                                Messege, ButtonEnum.Ok).ShowAsync();
                        }
                        else
                        {
                            Messege = "Пункт выдачи выбран! Заказ создан!";
                            ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Внимание! Выбор пункта выдачи!",
                                Messege, ButtonEnum.Ok).ShowAsync();

                            if (NewOrder.Orderid == 0)
                            {
                                NewOrder.Orderstatus = 1;
                                NewOrder.Orderdate = DateOnly.FromDateTime(DateTime.Now);
                                NewOrder.Orderclient = _currentUser.Userid;
                                NewOrder.Ordercodetoreceive = GenerateUniqueOrderCode();
                                NewOrder.Orderproducts = new List<Orderproduct>();
                            }
                            
                            IsCurrentOrder = true;

                            Messege = "Новый заказ создан!";
                            result = await MessageBoxManager.GetMessageBoxStandard("Создание заказа!",
                                Messege, ButtonEnum.Ok).ShowAsync();
                        }                        
                    }
                    catch (DbUpdateException dbEx)
                    {
                        foreach (var entry in dbEx.Entries)
                        {
                            Console.WriteLine($"Ошибка при обновлении {entry.Entity.GetType().Name} в состоянии {entry.State}.");
                        }
                        await MessageBoxManager.GetMessageBoxStandard("Ошибка при обновлении БД", dbEx.InnerException?.Message ?? dbEx.Message, ButtonEnum.Ok).ShowAsync();
                    }
                }

                if (NewOrder.OrderpickuppointNavigation == null)
                {
                    Messege = "Выберите пункт выдачи! Товар не добавлен, так как заказ не создан!";
                    ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Внимание! Ошибка пункта выдачи!",
                        Messege, ButtonEnum.Ok).ShowAsync();
                }
                else
                {
                    Orderproduct? existingOrderProduct = NewOrder.Orderproducts.FirstOrDefault(x => x.Productarticlenumber == product.Productarticlenumber);

                    if (existingOrderProduct != null)
                    {
                        existingOrderProduct.Productquantity += 1;
                    }
                    else
                    {
                        NewOrder.Orderproducts.Add(new Orderproduct
                        {
                            Orderid = NewOrder.Orderid,
                            Productarticlenumber = product.Productarticlenumber,
                            Productquantity = 1
                        });
                    }

                    IsVisibleBTCurrentOrder = NewOrder.Orderproducts.Any(x => x.Productquantity > 0);

                    if (NewOrder.Orderproducts.Any(x => x.Productarticlenumber == product.Productarticlenumber &&  product.Productquantityinstock < 3))
                    {
                        NewOrder.Orderdeliverydate = DateOnly.FromDateTime(DateTime.Now.AddDays(6));
                    }
                    else
                    {
                        NewOrder.Orderdeliverydate = DateOnly.FromDateTime(DateTime.Now.AddDays(3));
                    }

                    Messege = $"Товар '{product.Productname}' добавлен в текущий заказ!";
                    ButtonResult result1 = await MessageBoxManager.GetMessageBoxStandard("Добавление товара к заказу!",
                        Messege, ButtonEnum.Ok).ShowAsync();
                }            
            }
            catch (DbUpdateException dbEx)
            {
                foreach (var entry in dbEx.Entries)
                {
                    Console.WriteLine($"Ошибка при обновлении {entry.Entity.GetType().Name} в состоянии {entry.State}.");
                }
                await MessageBoxManager.GetMessageBoxStandard("Ошибка при обновлении БД", dbEx.InnerException?.Message ?? dbEx.Message, ButtonEnum.Ok).ShowAsync();
            }
        }

        #endregion
    }
}
