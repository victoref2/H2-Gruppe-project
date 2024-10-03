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

        // Properties
        private DateTime _closingDate;
        private string _currentBid;

        private readonly User _loggedInUser;
        private readonly Database _database;

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

        public AuctionSellerViewModel(MainWindowViewModel mainWindowViewModel, User loggedInUser, Database database)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _loggedInUser = loggedInUser;
            _database = database;

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

        [RelayCommand]
        public void GoBack()
        {
            _mainWindowViewModel.SwitchViewModel(new DashboardViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }
    }
}
