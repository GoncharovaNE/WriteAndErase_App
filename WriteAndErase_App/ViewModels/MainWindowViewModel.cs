using Avalonia.Controls;
using ReactiveUI;
using WriteAndErase_App.Models;

namespace WriteAndErase_App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static MainWindowViewModel Instance;

        public static PostgresContext myСonnection = new PostgresContext();

        public MainWindowViewModel()
        {
            Instance = this;
        }

        private UserControl _contentPage = new AuthorizationPage();

        public UserControl ContentPage { get => _contentPage; set => this.RaiseAndSetIfChanged(ref _contentPage, value); }

        //Scaffold-DbContext "Host=localhost;Port=5433;Database=postgres;Username=postgres;Password=123456" -f -o Models Npgsql.EntityFrameworkCore.PostgreSQL
    }
}
