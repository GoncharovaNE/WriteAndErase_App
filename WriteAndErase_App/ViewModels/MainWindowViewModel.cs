using Avalonia.Controls;
using ReactiveUI;
using WriteAndErase_App.Models;

namespace WriteAndErase_App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel Instance;

        public PostgresContext myСonnection = new PostgresContext();

        public MainWindowViewModel()
        {
            Instance = this;
        }

        private UserControl _contentPage = new AuthorizationPage();

        public UserControl ContentPage { get => _contentPage; set => this.RaiseAndSetIfChanged(ref _contentPage, value); }
    }
}
