using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;
using System.Threading.Tasks;
using System.Linq;

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
            //LoadAuctions();
        }

        /*private void LoadAuctions()
        {
            YourAuctions = new ObservableCollection<Auction>(_database.GetUserAuctions(_loggedInUser.Id));
            CurrentAuctions = new ObservableCollection<Auction>(_database.GetAllAuctions());
        }*/

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


        [RelayCommand]
        public void OpenSelectedAuction()
        {
            // Find the selected auction in YourAuctions or CurrentAuctions
            var selectedAuction = YourAuctions.FirstOrDefault(a => a.IsSelected) ??
                                  CurrentAuctions.FirstOrDefault(a => a.IsSelected);

            if (selectedAuction == null)
            {
                // Handle case where no auction is selected
                return;
            }

            // Check if this is the user's own auction
            if (selectedAuction.Seller.Id == _loggedInUser.Id)
            {
                // Navigate to the Auction Seller page
                _mainWindowViewModel.SwitchViewModel(new AuctionSellerViewModel(_mainWindowViewModel, _loggedInUser, _database, selectedAuction));
            }
            else
            {
                // Navigate to the Auction Buyer page
                _mainWindowViewModel.SwitchViewModel(new AuctionBuyingViewModel(_mainWindowViewModel, _loggedInUser, _database, selectedAuction));
            }


        }

    }
}
