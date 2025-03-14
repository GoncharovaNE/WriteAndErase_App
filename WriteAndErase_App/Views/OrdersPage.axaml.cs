using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteAndErase_App.ViewModels;
using WriteAndErase_App.Models;

namespace WriteAndErase_App;

public partial class OrdersPage : UserControl
{
    public OrdersPage()
    {
        InitializeComponent();
        DataContext = new OrdersVM();
    }

    public OrdersPage(User currentUser)
    {
        InitializeComponent();
        DataContext = new OrdersVM(currentUser);
    }

    private void OnOrderSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (DataContext is OrdersVM vm && e.AddedItems.Count > 0 && e.AddedItems[0] is Order order)
        {
            vm.ToEditOrder(order.Orderid);
        }
    }
}