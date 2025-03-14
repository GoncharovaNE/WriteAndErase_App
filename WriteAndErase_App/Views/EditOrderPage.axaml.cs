using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteAndErase_App.Models;
using WriteAndErase_App.ViewModels;

namespace WriteAndErase_App;

public partial class EditOrderPage : UserControl
{
    public EditOrderPage()
    {
        InitializeComponent();
        DataContext = new EditOrderVM();
    }

    public EditOrderPage(int id, User currentUser)
    {
        InitializeComponent();
        DataContext = new EditOrderVM(id, currentUser);
    }
}