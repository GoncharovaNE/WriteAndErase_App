using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteAndErase_App.ViewModels;

namespace WriteAndErase_App;

public partial class ProductPage : UserControl
{
    public ProductPage(int id)
    {
        InitializeComponent();
        DataContext = new ProductVM(id);
    }
}