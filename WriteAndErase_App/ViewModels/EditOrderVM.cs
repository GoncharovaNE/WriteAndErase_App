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
    class EditOrderVM : ViewModelBase
    {
        private string _user;

        public string User { get => _user; set => this.RaiseAndSetIfChanged(ref _user, value); }

        private User? _currentUser;

        public User? CurrentUser { get => _currentUser; set => this.RaiseAndSetIfChanged(ref _currentUser, value); }               
        
        private List<Status> _listOrderStatus;

        public List<Status> ListOrderStatus { get => _listOrderStatus; set => this.RaiseAndSetIfChanged(ref _listOrderStatus, value); }        

        private Order? _newOrder;

        public Order? NewOrder { get => _newOrder; set => this.RaiseAndSetIfChanged(ref _newOrder, value); }

        public EditOrderVM()
        {
            _listOrderStatus = MainWindowViewModel.myСonnection.Statuses.ToList();
        }

        public EditOrderVM(int id, User currentUser)
        {
            _currentUser = currentUser;
            _user = _currentUser?.Userid == 1 ? "Гость" : $"{_currentUser?.Username} {_currentUser?.Usersurname} {_currentUser?.Userpatronymic}";

            _newOrder = MainWindowViewModel.myСonnection.Orders
                        .FirstOrDefault(x => x.Orderid == id);

            _listOrderStatus = MainWindowViewModel.myСonnection.Statuses.ToList();   
        }

        public void ToBack()
        {
            MainWindowViewModel.myСonnection.Entry(NewOrder).Reload();
            MainWindowViewModel.Instance.ContentPage = new OrdersPage(CurrentUser);
        }

        public async void SaveOrder()
        {
            try
            {
                if (NewOrder == null)
                {
                    await ShowMessage("Ошибка!", "Заказ не найден.");
                }
                else if (NewOrder.Orderdeliverydate < DateOnly.MinValue && NewOrder.Orderdeliverydate < NewOrder.Orderdate)
                {
                    await ShowMessage("Ошибка!", "Дата доставки должна быть позже минимальной даты и позже даты оформления заказа.");
                }
                else
                {
                    MainWindowViewModel.myСonnection.SaveChanges();
                    await ShowMessage("Успех!", "Изменения успешно сохранены.");
                    MainWindowViewModel.Instance.ContentPage = new OrdersPage(CurrentUser);
                }                    
            }
            catch (Exception ex)
            {
                await ShowMessage("Ошибка!", ex.Message);
            }
        }

        private async Task ShowMessage(string title, string message)
        {
            await MessageBoxManager.GetMessageBoxStandard(title, message, ButtonEnum.Ok).ShowAsync();
        }
    }
}
