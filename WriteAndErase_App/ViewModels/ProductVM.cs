using ReactiveUI;
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

        public ProductVM(int id)
        {
            if (id == 1)
            {
                _user = "Гость";
            }
            else
            {
                _listUser = MainWindowViewModel.myСonnection.Users.ToList();
                string fio = $"{_listUser.FirstOrDefault(x => x.Userid == id).Username} {_listUser.FirstOrDefault(x => x.Userid == id).Usersurname} {_listUser.FirstOrDefault(x => x.Userid == id).Userpatronymic}";
                _user = $"{fio}";
            }
        }

        public void ToBackAuth()
        {
            MainWindowViewModel.Instance.ContentPage = new AuthorizationPage();
        }

        
    }
}
