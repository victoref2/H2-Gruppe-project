using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;
using System.Threading.Tasks;

namespace H2_Gruppe_project.ViewModels
{
    public partial class DashboardViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private ObservableCollection<Auction> yourAuctions; // No need for "?" (nullability)

        [ObservableProperty]
        private ObservableCollection<Auction> currentAuctions; // No need for "?" (nullability)

        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly User _loggedInUser;
        private readonly Database _database;

        public DashboardViewModel(MainWindowViewModel mainWindowViewModel, User loggedInUser, Database database)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _loggedInUser = loggedInUser;
            _database = database;

            UserName = _loggedInUser.Name;

            // Initialize collections
            LoadAuctions();
        }

        private void LoadAuctions()
        {
            YourAuctions = new ObservableCollection<Auction>(_database.GetUserAuctions(_loggedInUser.Id));
            CurrentAuctions = new ObservableCollection<Auction>(_database.GetAllAuctions());
        }

        [RelayCommand]
        public void GoToAddVehicle()
        {
            _mainWindowViewModel.SwitchViewModel(new AddVHViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }

        [RelayCommand]
        public void GoToProfile()
        {
            _mainWindowViewModel.SwitchViewModel(new ProfileViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }

        [RelayCommand]
        public void Logout()
        {
            _mainWindowViewModel.SwitchViewModel(new LoginViewModel(_mainWindowViewModel, _database));
        }

        [RelayCommand]
        public void GoToAuctionHistory()
        {
            // Implement navigation to Auction history page if needed.
        }
    }
}
