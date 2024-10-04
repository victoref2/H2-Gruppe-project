using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;

namespace H2_Gruppe_project.ViewModels
{
    public partial class AuctionBuyingViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        // Auction data
        public Auction Auction { get; set; }

        private bool _isBidWindowVisible;
        private string _bidAmount;

        private readonly User _loggedInUser;
        private readonly Database _database;

        // Property to show/hide bid window
        public bool IsBidWindowVisible
        {
            get => _isBidWindowVisible;
            set => SetProperty(ref _isBidWindowVisible, value);
        }

        // Property for Bid Amount
        public string BidAmount
        {
            get => _bidAmount;
            set => SetProperty(ref _bidAmount, value);
        }

        // Commands
        public IRelayCommand MakeBidCommand { get; }
        public IRelayCommand CancelBidCommand { get; }
        public IRelayCommand SubmitBidCommand { get; }
        public IRelayCommand BackCommand { get; }

        public AuctionBuyingViewModel(MainWindowViewModel mainWindowViewModel, User loggedInUser, Database database, Auction auction)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _loggedInUser = loggedInUser;
            _database = database;

            // Auction data passed from Dashboard
            Auction = auction;

            // Initialize properties
            BidAmount = string.Empty;

            // Initialize commands
            MakeBidCommand = new RelayCommand(OpenBidWindow);
            CancelBidCommand = new RelayCommand(CloseBidWindow);
            SubmitBidCommand = new RelayCommand(SubmitBid);
            BackCommand = new RelayCommand(GoBack);
        }

        // Command methods
        private void OpenBidWindow()
        {
            IsBidWindowVisible = true;
        }

        private void CloseBidWindow()
        {
            IsBidWindowVisible = false;
        }

        private void SubmitBid()
        {
            if (decimal.TryParse(BidAmount, out decimal bidValue))
            {
                // Logic for submitting the bid
                Auction.CurrentPrice = bidValue; // Update the current price in the auction object
                CloseBidWindow(); // Close the bid window after successful bid
            }
            else
            {
                // Handle invalid bid (optional: show an error message)
            }
        }

        private void GoBack()
        {
            _mainWindowViewModel.SwitchViewModel(new DashboardViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }
    }
}
