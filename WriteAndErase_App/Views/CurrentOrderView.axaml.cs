using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteAndErase_App.ViewModels;

namespace WriteAndErase_App;

public partial class CurrentOrderView : UserControl
{
    public CurrentOrderView()
    {
        InitializeComponent();
        DataContext = new CurrentOrderVM();
    }
}