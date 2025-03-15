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
    /// <summary>
    /// ViewModel для редактирования заказа.
    /// </summary>
    class EditOrderVM : ViewModelBase
    {
        /// <summary>
        /// ФИО пользователя, работающий с заказом.
        /// </summary>
        private string _user;
        public string User { get => _user; set => this.RaiseAndSetIfChanged(ref _user, value); }

        /// <summary>
        /// Текущий пользователь, работающий с заказом.
        /// </summary>
        private User? _currentUser;
        public User? CurrentUser { get => _currentUser; set => this.RaiseAndSetIfChanged(ref _currentUser, value); }

        /// <summary>
        /// Список статусов заказа.
        /// </summary>
        private List<Status> _listOrderStatus;
        public List<Status> ListOrderStatus { get => _listOrderStatus; set => this.RaiseAndSetIfChanged(ref _listOrderStatus, value); }

        /// <summary>
        /// Текущий редактируемый заказ.
        /// </summary>
        private Order? _newOrder;
        public Order? NewOrder { get => _newOrder; set => this.RaiseAndSetIfChanged(ref _newOrder, value); }

        /// <summary>
        /// Конструктор по умолчанию, загружает список статусов заказов.
        /// </summary>
        public EditOrderVM()
        {
            _listOrderStatus = MainWindowViewModel.myСonnection.Statuses.ToList();
        }

        /// <summary>
        /// Конструктор с параметрами, загружает заказ по идентификатору и подгружает текущего пользователя.
        /// </summary>
        /// <param name="id">Идентификатор заказа.</param>
        /// <param name="currentUser">Текущий пользователь.</param>
        public EditOrderVM(int id, User currentUser)
        {
            _currentUser = currentUser;
            _user = _currentUser?.Userid == 1 ? "Гость" : $"{_currentUser?.Username} {_currentUser?.Usersurname} {_currentUser?.Userpatronymic}";

            _newOrder = MainWindowViewModel.myСonnection.Orders
                        .FirstOrDefault(x => x.Orderid == id);

            _listOrderStatus = MainWindowViewModel.myСonnection.Statuses.ToList();   
        }

        /// <summary>
        /// Метод для возврата к списку заказов.
        /// </summary>
        public void ToBack()
        {
            MainWindowViewModel.myСonnection.Entry(NewOrder).Reload();
            MainWindowViewModel.Instance.ContentPage = new OrdersPage(CurrentUser);
        }

        /// <summary>
        /// Метод для сохранения изменений в заказе.
        /// </summary>
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

        /// <summary>
        /// Метод для отображения сообщения пользователю.
        /// </summary>
        /// <param name="title">Заголовок сообщения.</param>
        /// <param name="message">Текст сообщения.</param>
        private async Task ShowMessage(string title, string message)
        {
            await MessageBoxManager.GetMessageBoxStandard(title, message, ButtonEnum.Ok).ShowAsync();
        }
    }
}
