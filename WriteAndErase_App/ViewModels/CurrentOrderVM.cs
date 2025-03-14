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
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.IO;

namespace WriteAndErase_App.ViewModels
{
    class CurrentOrderVM : ViewModelBase
    {
        
        #region Свойства для текущего заказа

        private ObservableCollection<Orderproduct> _listCurrentOrderProducts;
        public ObservableCollection<Orderproduct> ListCurrentOrderProducts
        {
            get => _listCurrentOrderProducts;
            set => this.RaiseAndSetIfChanged(ref _listCurrentOrderProducts, value);
        }

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

        private bool _orderFormed = false;

        public bool OrderFormed { get => _orderFormed; set => this.RaiseAndSetIfChanged(ref _orderFormed, value); }

        private bool _ticketFormed = false;

        public bool TicketFormed { get => _ticketFormed; set => this.RaiseAndSetIfChanged(ref _ticketFormed, value); }

        #endregion

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

        private bool _isForAdminMeneg = false;

        public bool IsForAdminMeneg { get => _isForAdminMeneg; set => this.RaiseAndSetIfChanged(ref _isForAdminMeneg, value); }

        #endregion

        public void ToBackProduct()
        {
            MainWindowViewModel.Instance.ContentPage = new ProductPage(CurrentUser.Userid, _currentOrder, IsVisibleBTCurrentOrder, IsCurrentOrder, IsForAdminMeneg);
        }

        public async void CompleteCurrentOrder()
        {
            if (OrderFormed != true)
            {
                ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Предупреждение!", "Заказ не оформлен! Вы уверены что хотите завершить текущий заказ? " +
                    "После согласия все данные о текущем заказе будут стёрты!", ButtonEnum.YesNo).ShowAsync();

                switch(result)
                {
                    case ButtonResult.Yes:
                        {
                            ClearNewOrder();
                            MessageBoxManager.GetMessageBoxStandard("Внимание!", "Заказ не оформлен! Текущий заказ завершён!", ButtonEnum.Ok).ShowAsync();
                            MainWindowViewModel.Instance.ContentPage = new ProductPage(CurrentUser.Userid, _currentOrder, IsVisibleBTCurrentOrder, IsCurrentOrder, IsForAdminMeneg);
                            break;
                        }

                    case ButtonResult.No:
                        {
                            MessageBoxManager.GetMessageBoxStandard("Внимание!", "Заказ не оформлен! Текущий заказ не завершён!", ButtonEnum.Ok).ShowAsync();
                            break;
                        }
                }
            }
            else if (OrderFormed == true && TicketFormed != true)
            {
                ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Предупреждение!", "Заказ оформлен, но не сохранён талон! Вы уверены что хотите завершить текущий заказ? " +
                    "После согласия вы не сможете получить талон на текущий заказ!", ButtonEnum.YesNo).ShowAsync();

                switch (result)
                {
                    case ButtonResult.Yes:
                        {
                            ClearNewOrder();
                            MessageBoxManager.GetMessageBoxStandard("Внимание!", "Заказ оформлен, но не сохранён талон! Текущий заказ завершён!", ButtonEnum.Ok).ShowAsync();
                            MainWindowViewModel.Instance.ContentPage = new ProductPage(CurrentUser.Userid, _currentOrder, IsVisibleBTCurrentOrder, IsCurrentOrder, IsForAdminMeneg);
                            break;
                        }

                    case ButtonResult.No:
                        {
                            MessageBoxManager.GetMessageBoxStandard("Внимание!", "Заказ оформлен, но не сохранён талон! Текущий заказ не завершён!", ButtonEnum.Ok).ShowAsync();
                            break;
                        }
                }
            }
            else if (OrderFormed == true && TicketFormed == true)
            {
                ClearNewOrder();
                MessageBoxManager.GetMessageBoxStandard("Внимание!", "Заказ оформлен! Талон сохранён! Текущий заказ завершён!", ButtonEnum.Ok).ShowAsync();
                MainWindowViewModel.Instance.ContentPage = new ProductPage(CurrentUser.Userid, _currentOrder, IsVisibleBTCurrentOrder, IsCurrentOrder, IsForAdminMeneg);
            }
            else
            {
                MessageBoxManager.GetMessageBoxStandard("Внимание!", "Случилась какая-то ошибка!", ButtonEnum.YesNo).ShowAsync();
            }
        }

        public CurrentOrderVM()
        {

        }

        public CurrentOrderVM(User CurrentUser, Order newOrder, bool IsVisibleBTCurrentOrder, bool IsCurrentOrder, bool IsAdminMeneg)
        {
            _listCurrentOrderProducts = new ObservableCollection<Orderproduct>(newOrder.Orderproducts
            .Select(op => new Orderproduct
            {
                Productarticlenumber = op.Productarticlenumber,
                Productquantity = op.Productquantity,
                Order = newOrder,
                ProductarticlenumberNavigation = MainWindowViewModel.myСonnection.Products
                    .Include(p => p.ProductunitofmeasurementNavigation)
                    .FirstOrDefault(p => p.Productarticlenumber == op.Productarticlenumber)
            }));

            _currentOrder = newOrder;

            _currentUser = CurrentUser;

            IsForAdminMeneg = _currentUser.Userrole == 2 || _currentUser.Userrole == 3 ? true : false;

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

            TotalProductQuantity = ListCurrentOrderProducts.Sum(x => x.Productquantity);
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

                    CurrentOrder.Orderproducts = ListCurrentOrderProducts;

                    foreach (Orderproduct? product in CurrentOrder.Orderproducts)
                    {
                        if (product.Productquantity > 0)
                        {
                            MainWindowViewModel.myСonnection.Orderproducts.Add(product);
                        }
                    }

                    MainWindowViewModel.myСonnection.SaveChanges();

                    OrderFormed = true;

                    MessageBoxManager.GetMessageBoxStandard("Успех!", "Заказ оформлен!", ButtonEnum.Ok).ShowAsync();                                    
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

        public void ClearNewOrder()
        {
            CurrentOrder = new Order();
            ListCurrentOrderProducts = new ObservableCollection<Orderproduct>();

            OrderDate = string.Empty;
            OrderDateDelivery = string.Empty;
            OrderCode = 0;
            OrderSum = 0;
            OrderDiscount = 0;
            FinalOrderSum = 0;
            PickupPoint = string.Empty;
            DeliveryTime = string.Empty;
            IsVisibleBTCurrentOrder = false;
            IsCurrentOrder = false;
        }

        public void GenerateOrderTicket()
        {
            try
            {
                if (OrderFormed != true)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка!", "Нельзя сформировать талон, если не сформирован заказ!", ButtonEnum.Ok).ShowAsync();
                }
                else
                {
                    // Создание документа
                    Document document = new Document();
                    Section section = document.AddSection();

                    // Заголовок
                    Paragraph title = section.AddParagraph("ТАЛОН НА ЗАКАЗ");
                    title.Format.Font.Size = 16;
                    title.Format.Font.Bold = true;
                    title.Format.SpaceAfter = "10pt";
                    title.Format.Alignment = ParagraphAlignment.Center;

                    // Информация о заказе
                    section.AddParagraph($"Дата заказа: {OrderDate}");
                    section.AddParagraph($"Номер заказа: {CurrentOrder.Orderid}");
                    section.AddParagraph($"Пункт выдачи: {PickupPoint}");

                    // Код получения (жирный, крупный)
                    Paragraph orderCode = section.AddParagraph($"Код получения: {OrderCode}");
                    orderCode.Format.Font.Size = 14;
                    orderCode.Format.Font.Bold = true;
                    orderCode.Format.SpaceAfter = "10pt";

                    // Состав заказа
                    section.AddParagraph("Состав заказа:");
                    Table table = section.AddTable();
                    table.Borders.Width = 0.5;

                    Column column1 = table.AddColumn("3cm");
                    Column column2 = table.AddColumn("7cm");
                    Column column3 = table.AddColumn("3cm");

                    Row header = table.AddRow();
                    header.Cells[0].AddParagraph("Кол-во");
                    header.Cells[1].AddParagraph("Название");
                    header.Cells[2].AddParagraph("Цена");

                    foreach (var product in ListCurrentOrderProducts)
                    {
                        Row row = table.AddRow();
                        row.Cells[0].AddParagraph(product.Productquantity.ToString());
                        row.Cells[1].AddParagraph(product.ProductarticlenumberNavigation.Productname);
                        row.Cells[2].AddParagraph($"{product.ProductarticlenumberNavigation.Productcost} руб.");
                    }

                    // Итоги
                    section.AddParagraph($"Сумма заказа: {OrderSum} руб.");
                    section.AddParagraph($"Сумма скидки: {OrderDiscount} руб.");
                    section.AddParagraph($"Итоговая сумма: {FinalOrderSum} руб.");
                    section.AddParagraph($"Срок доставки: {DeliveryTime}");

                    // Рендеринг в PDF
                    PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                    renderer.Document = document;
                    renderer.RenderDocument();

                    // Определение пути к рабочему столу
                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string filename = Path.Combine(desktopPath, $"Талон_заказ_{CurrentOrder.Orderid}.pdf");

                    // Сохранение файла
                    renderer.PdfDocument.Save(filename);

                    TicketFormed = true;

                    MessageBoxManager.GetMessageBoxStandard("Успех!", $"Талон сохранён: {filename}", ButtonEnum.Ok).ShowAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBoxManager.GetMessageBoxStandard("Ошибка!", ex.Message, ButtonEnum.Ok).ShowAsync();
            }
        }

        #region изменение наличия товара в заказе

        private int _totalProductQuantity;
        public int TotalProductQuantity
        {
            get => _totalProductQuantity;
            set => this.RaiseAndSetIfChanged(ref _totalProductQuantity, value);
        }

        public void IncreaseQuantity(Orderproduct product)
        {
            if (product != null)
            {
                product.Productquantity++;
                UpdateProduct(product);
            }
        }

        public void DecreaseQuantity(Orderproduct product)
        {
            if (product != null)
            {
                if (product.Productquantity > 1)
                {
                    product.Productquantity--;
                }
                else
                {
                    ListCurrentOrderProducts.Remove(product);
                }
                UpdateProduct(product);
            }
        }

        public void RemoveProduct(Orderproduct product)
        {
            if (product != null)
            {
                ListCurrentOrderProducts.Remove(product);
                UpdateProduct(product);
            }
        }

        private void UpdateProduct(Orderproduct product)
        {
            int index = ListCurrentOrderProducts.IndexOf(product);
            if (index >= 0)
            {
                ListCurrentOrderProducts[index] = new Orderproduct
                {
                    Productarticlenumber = product.Productarticlenumber,
                    Productquantity = product.Productquantity,
                    ProductarticlenumberNavigation = product.ProductarticlenumberNavigation 
                };
            }

            UpdateOrderSummary();
        }

        public void UpdateOrderSummary()
        {
            OrderSum = ListCurrentOrderProducts.Sum(x => x.ProductarticlenumberNavigation.Productcost * x.Productquantity);
            OrderDiscount = Math.Round((double)ListCurrentOrderProducts.Sum(x => (x.ProductarticlenumberNavigation.Productcost * x.Productquantity) *
            (x.ProductarticlenumberNavigation.Productdiscountamount / 100)), 2);
            FinalOrderSum = Math.Max(OrderSum - OrderDiscount, 0);

            TotalProductQuantity = ListCurrentOrderProducts.Sum(x => x.Productquantity);

            if (ListCurrentOrderProducts.Count == 0)
            {
                ClearNewOrder();
                ToBackProduct();
            }
        }

        #endregion
    }
}
