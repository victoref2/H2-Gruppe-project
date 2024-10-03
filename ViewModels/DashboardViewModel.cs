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

        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly User _loggedInUser;
        private readonly Database _database;


        public DashboardViewModel(MainWindowViewModel mainWindowViewModel, User loggedInUser, Database database)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _loggedInUser = loggedInUser;
            _database = database;


            UserName = _loggedInUser.Name;
        }

        [RelayCommand]
        public void GoToAddVehicle()
        {
            _mainWindowViewModel.SwitchViewModel(new AddVHViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }

        [RelayCommand]
        public void GoToAuctionSellerModel()
        {
            _mainWindowViewModel.SwitchViewModel(new AuctionSellerViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }

        [RelayCommand]
        public void GoToAuctionBuyingModel()
        {
            _mainWindowViewModel.SwitchViewModel(new AuctionBuyingViewModel(_mainWindowViewModel, _loggedInUser, _database));
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
    }
}
