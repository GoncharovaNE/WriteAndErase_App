using Microsoft.EntityFrameworkCore;
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

        public string User { get => _user; set => this.RaiseAndSetIfChanged(ref _user, value); }
        
        private List<User> _listUser;

        public List<User> ListUser { get => _listUser; set => this.RaiseAndSetIfChanged(ref _listUser, value); }

        private List<Product> _listProduct;

        public List<Product> ListProduct { get => _listProduct; set => this.RaiseAndSetIfChanged(ref _listProduct, value); }


        public ProductVM()
        {            
            _listProduct = MainWindowViewModel.myСonnection.Products.Include(x => x.Productmanufacturers).ThenInclude(x => x.Manufacturer).ToList();
        }

        public ProductVM(int id)
        {
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
    }
}
