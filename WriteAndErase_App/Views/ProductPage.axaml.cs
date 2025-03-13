using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteAndErase_App.Models;
using WriteAndErase_App.ViewModels;

namespace WriteAndErase_App;

public partial class ProductPage : UserControl
{
    public ProductPage()
    {
        InitializeComponent();
        DataContext = new ProductVM();
    }

    public ProductPage(int id)
    {
        InitializeComponent();
        DataContext = new ProductVM(id);
    }

    public ProductPage(int id, Order currentOrder, bool IsVisibleBTCurrentOrder, bool IsEnablePickuppoint, bool IsCurrentOrder)
    {
        InitializeComponent();
        DataContext = new ProductVM(id, currentOrder, IsVisibleBTCurrentOrder, IsEnablePickuppoint, IsCurrentOrder);
    }
}