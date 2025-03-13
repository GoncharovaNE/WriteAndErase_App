using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteAndErase_App.Models;
using WriteAndErase_App.ViewModels;

namespace WriteAndErase_App;

public partial class CurrentOrderView : UserControl
{
    public CurrentOrderView()
    {
        InitializeComponent();
        DataContext = new CurrentOrderVM();
    }

    public CurrentOrderView(Order currentOrder, bool IsVisibleBTCurrentOrder, bool IsEnablePickuppoint, bool IsCurrentOrder)
    {
        InitializeComponent();
        DataContext = new CurrentOrderVM(currentOrder, IsVisibleBTCurrentOrder, IsEnablePickuppoint, IsCurrentOrder);
    }
}