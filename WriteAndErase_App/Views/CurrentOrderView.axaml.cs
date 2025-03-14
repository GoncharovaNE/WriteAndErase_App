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

    public CurrentOrderView(User CurrentUser, Order CurrentOrder, bool IsVisibleBTCurrentOrder, bool IsCurrentOrder)
    {
        InitializeComponent();
        DataContext = new CurrentOrderVM(CurrentUser, CurrentOrder, IsVisibleBTCurrentOrder, IsCurrentOrder);
    }
}