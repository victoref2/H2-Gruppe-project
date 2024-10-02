using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using H2_Gruppe_project.DatabaseClasses;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.ViewModels
{
    public partial class ProfileViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private decimal balance;

        [ObservableProperty]
        private int auctionCount;

        [ObservableProperty]
        private int wonAuctionCount;

        [ObservableProperty]
        private string successMessage;

        private readonly Database _database;


        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly User _loggedInUser;

        public bool IsSuccessMessageVisible => !string.IsNullOrWhiteSpace(SuccessMessage);


        public ProfileViewModel(MainWindowViewModel mainWindowViewModel, User loggedInUser, Database database, string successMessage = null)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _loggedInUser = loggedInUser;
            _database = database;


            Username = _loggedInUser.Name;
            Balance = _loggedInUser.Balance;
            AuctionCount = 0;
            WonAuctionCount = 0;


            SuccessMessage = successMessage;
        }

        [RelayCommand]
        public void GoBack()
        {
            _mainWindowViewModel.SwitchViewModel(new DashboardViewModel(_mainWindowViewModel, _loggedInUser, _database)); // Logic to navigate back
        }
        [RelayCommand]
        public void ChangePassword()
        {
            _mainWindowViewModel.SwitchViewModel(new ChangePasswordViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }
    }
}
