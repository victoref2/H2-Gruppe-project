using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace H2_Gruppe_project.ViewModels
{
    public partial class AuctionSellerModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        // Properties
        private DateTime _closingDate;
        private string _currentBid;

        public string CurrentBid
        {
            get => _currentBid;
            set => SetProperty(ref _currentBid, value);
        }

        public DateTime ClosingDate
        {
            get => _closingDate;
            set => SetProperty(ref _closingDate, value);
        }

        // Commands
        public IRelayCommand AcceptBidCommand { get; }
        public IRelayCommand BackCommand { get; }

        public AuctionSellerModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;

            // Initialize properties
            ClosingDate = new DateTime(2022, 12, 12); // Example closing date
            CurrentBid = "DKK 568.000"; // Example bid amount

            // Initialize commands
            AcceptBidCommand = new RelayCommand(AcceptBid);
            BackCommand = new RelayCommand(GoBack);
        }

        // Command methods
        private void AcceptBid()
        {
            // Logic to accept the current bid, e.g., notify the system that the bid is accepted
            // You can add your logic here, like communicating with a database or another service
            Console.WriteLine("Bid Accepted");
        }

        private void GoBack()
        {
            // Logic to navigate back to the previous page or the main auction page
            Console.WriteLine("Navigating back");
        }
    }
}
