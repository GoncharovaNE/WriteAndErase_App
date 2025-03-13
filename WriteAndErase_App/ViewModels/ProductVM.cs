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
        private string _user;

        private int _userId;

        private List<User> _listUser;

        private List<Product> _listProduct;        

        public string User { get => _user; set => this.RaiseAndSetIfChanged(ref _user, value); }

        public int UserId { get => _userId; set => this.RaiseAndSetIfChanged(ref _userId, value); }        

        public List<User> ListUser { get => _listUser; set => this.RaiseAndSetIfChanged(ref _listUser, value); }

        public List<Product> ListProduct { get => _listProduct; set => this.RaiseAndSetIfChanged(ref _listProduct, value); }        

        public ProductVM()
        {            
            _listProduct = MainWindowViewModel.myСonnection.Products.Include(x => x.Productmanufacturers).ThenInclude(x => x.Manufacturer).ToList();
        }

        public ProductVM(int id)
        {
            _userId = id;
            _listOrder = MainWindowViewModel.myСonnection.Orders.ToList();
            if (id == 1)
            {
                _user = "Гость";
            }
            else
            {
                _listUser = MainWindowViewModel.myСonnection.Users.ToList();
                User? userId = _listUser.FirstOrDefault(x => x.Userid == id);
                string fio = $"{userId?.Username} {userId?.Usersurname} {userId?.Userpatronymic}";
                _user = $"{fio}";
            }

            _listProduct = MainWindowViewModel.myСonnection.Products.Include(x => x.Productmanufacturers).ThenInclude(x => x.Manufacturer).ToList();
        }

        public void ToBackAuth()
        {
            MainWindowViewModel.Instance.ContentPage = new AuthorizationPage();
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

        private List<Order> _listOrder;

        public bool IsCurrentOrder { get => _IsCurrentOrder; set => this.RaiseAndSetIfChanged(ref _IsCurrentOrder, value); }

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
                        if (NewOrder.Orderid == 0)
                        {
                            MainWindowViewModel.myСonnection.Orders.Add(NewOrder);
                        }
                        NewOrder.Orderstatus = 1;
                        NewOrder.Orderdate = DateOnly.FromDateTime(DateTime.Now);
                        NewOrder.Orderclient = _userId;
                        NewOrder.Ordercodetoreceive = GenerateUniqueOrderCode();

                        MainWindowViewModel.myСonnection.SaveChanges();                        

                        _IsCurrentOrder = true;

                        Messege = "Новый заказ создан!";
                        ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Создание заказа!",
                            Messege, ButtonEnum.Ok).ShowAsync();
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

                Orderproduct? existingOrderProduct = MainWindowViewModel.myСonnection.Orderproducts
                    .FirstOrDefault(x => x.Orderid == NewOrder.Orderid && x.Productarticlenumber == product.Productarticlenumber);

                if (existingOrderProduct != null)
                {
                    existingOrderProduct.Productquantity += 1;
                }
                else
                {
                    Orderproduct NewOrderProduct = new Orderproduct();
                    NewOrderProduct.Orderid = NewOrder.Orderid;
                    NewOrderProduct.Productarticlenumber = product.Productarticlenumber;
                    NewOrderProduct.Productquantity = 1;

                    MainWindowViewModel.myСonnection.Orderproducts.Add(NewOrderProduct);
                }

                MainWindowViewModel.myСonnection.SaveChanges();

                IsVisibleBTCurrentOrder = MainWindowViewModel.myСonnection.Orderproducts
                    .Any(x => x.Orderid == NewOrder.Orderid && x.Productquantity > 0);

                Messege = "Товар добавлен в текущий заказ!";
                ButtonResult result1 = await MessageBoxManager.GetMessageBoxStandard("Добавление товара к заказу!",
                    Messege, ButtonEnum.Ok).ShowAsync();
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
