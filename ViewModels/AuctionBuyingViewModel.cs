using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;

namespace H2_Gruppe_project.ViewModels
{
    public partial class AuctionBuyingViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        // Properties
        private DateTime _closingDate;
        private string _currentBid;
        private bool _isBidWindowVisible;
        private string _bidAmount;

        private readonly User _loggedInUser;
        private readonly Database _database;

        // Property for CurrentBid
        public string CurrentBid
        {
            get => _currentBid;
            set => SetProperty(ref _currentBid, value);
        }

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
        public DateTime ClosingDate
        {
            get => _closingDate;
            set => SetProperty(ref _closingDate, value);
        }

        // Commands
        public IRelayCommand MakeBidCommand { get; }
        public IRelayCommand CancelBidCommand { get; }
        public IRelayCommand SubmitBidCommand { get; }
        public IRelayCommand BackCommand { get; }


        public AuctionBuyingViewModel(MainWindowViewModel mainWindowViewModel, User loggedInUser, Database database)
        {
            _mainWindowViewModel = mainWindowViewModel;

            _loggedInUser = loggedInUser;
            _database = database;


            // Initialize properties
            ClosingDate = new DateTime(2022, 12, 12);
            CurrentBid = "DKK 568.000"; // Example bid amount, can be data-bound
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
                CurrentBid = $"DKK {bidValue:N0}"; // Update current bid on the UI
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
