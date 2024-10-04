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
                // Check if bid is higher than current price
                if (bidValue <= Auction.CurrentPrice)
                {
                    // Bid must be higher than the current price
                    ShowErrorMessage("Bid must be higher than the current price.");
                    return;
                }

                // Check if user has enough balance
                if (_loggedInUser.Balance < bidValue)
                {
                    // User does not have enough money to place the bid
                    ShowErrorMessage("You do not have enough balance to place this bid.");
                    return;
                }

                // If there is a current buyer, refund their previous bid
                if (Auction.CurrentBuyer != null)
                {
                    // Refund the previous highest bidder
                    Auction.CurrentBuyer.Balance += Auction.CurrentPrice;
                    _database.UpdateUserBalance(Auction.CurrentBuyer.Id, Auction.CurrentBuyer.Balance);
                }

                // Deduct the new bid amount from the logged-in user's balance
                _loggedInUser.Balance -= bidValue;

                // Update the auction's current price and buyer
                Auction.CurrentPrice = bidValue;
                Auction.CurrentBuyer = _loggedInUser;
                    
                // Update the auction and user balance in the database
                _database.UpdateAuction(Auction);
                _database.UpdateUserBalance(_loggedInUser.Id, _loggedInUser.Balance);

                CloseBidWindow(); // Close the bid window after a successful bid
            }
            else
            {
                // Handle invalid bid (optional: show an error message)
                ShowErrorMessage("Invalid bid amount.");
            }
        }



        private void GoBack()
        {
            _mainWindowViewModel.SwitchViewModel(new DashboardViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }

        // Utility method to show an error message (can be replaced with actual UI error handling)
        private void ShowErrorMessage(string message)
        {
            Console.WriteLine(message); // Placeholder for real error message handling
        }
    }
}
