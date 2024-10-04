using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;

namespace H2_Gruppe_project.ViewModels
{
    public partial class AuctionSellerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        // Auction data
        public Auction Auction { get; set; }

        private readonly User _loggedInUser;
        private readonly Database _database;

        // Commands
        public IRelayCommand AcceptBidCommand { get; }
        public IRelayCommand BackCommand { get; }

        public AuctionSellerViewModel(MainWindowViewModel mainWindowViewModel, User loggedInUser, Database database, Auction auction)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _loggedInUser = loggedInUser;
            _database = database;

            // Set Auction data from the passed object
            Auction = auction;

            // Initialize commands
            AcceptBidCommand = new RelayCommand(AcceptBid);
            BackCommand = new RelayCommand(GoBack);
        }

        // Command methods
        private void AcceptBid()
        {
            // Logic to accept the current bid
            Auction.CurrentPrice = Auction.CurrentPrice; // This line will trigger acceptance logic
            Console.WriteLine("Bid Accepted");
            // Here, you'd likely also communicate with the database to update the auction status
        }

        [RelayCommand]
        public void GoBack()
        {
            _mainWindowViewModel.SwitchViewModel(new DashboardViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }
    }
}
