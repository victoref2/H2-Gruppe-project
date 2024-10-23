using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;
using System.Collections.Generic;
using HarfBuzzSharp;

namespace H2_Gruppe_project.ViewModels
{
    public partial class AuctionBuyingViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        // Auction data
        public Auction Auction { get; set; }

        private bool _isBidWindowVisible;
        private decimal _bidAmount;

        private readonly User _loggedInUser;
        private readonly Database _database;

        // Property to show/hide bid window
        public bool IsBidWindowVisible
        {
            get => _isBidWindowVisible;
            set => SetProperty(ref _isBidWindowVisible, value);
        }

        // Property for Bid Amount
        public decimal BidAmount
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
            BidAmount = 0;

            // Initialize commands
            MakeBidCommand = new RelayCommand(OpenBidWindow);
            CancelBidCommand = new RelayCommand(CloseBidWindow);
            SubmitBidCommand = new RelayCommand(SubmitBid);
            BackCommand = new RelayCommand(GoBack);

            VehicleData(Auction.Vehicle.Id);
        }

        // Visibility for Vehicle Types
        [ObservableProperty]
        private bool isTruckVisible;

        [ObservableProperty]
        private bool isBusVisible;

        [ObservableProperty]
        private bool isHeavyVehicleVisible;

        [ObservableProperty]
        private bool isNormalVHVisible;

        [ObservableProperty]
        private bool isPrivateVisible;

        [ObservableProperty]
        private bool isCommercialVisible;

        // Vehicle Type Specific Properties
        [ObservableProperty]
        private bool isofixMount;

        [ObservableProperty]
        private bool towBar;

        [ObservableProperty]
        private int numberOfSeats;

        [ObservableProperty]
        private string trunkDimensions;

        [ObservableProperty]
        private bool isCommercialVH;

        [ObservableProperty]
        private bool rollCage;

        [ObservableProperty]
        private decimal loadCapacity;

        [ObservableProperty]
        private int maxLoadCapacity;

        [ObservableProperty]
        private int numberOfAxles;

        [ObservableProperty]
        private int numberOfSleepingPlaces;

        [ObservableProperty]
        private bool hasToilet;

        [ObservableProperty]
        private decimal height;

        [ObservableProperty]
        private decimal length;

        [ObservableProperty]
        private decimal weight;

        [ObservableProperty]
        private string engineSize;
        public List<string> VehicleTypes { get; } = new List<string> { "Truck", "Bus", "CommercialVehicle", "PrivateVehicle" };

        [ObservableProperty]
        private string selectedVehicleType;

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
            if (BidAmount != null && BidAmount > Auction.CurrentPrice)
            {
                User buyer = _database.GetUserById(_loggedInUser.Id);
                CorporateUser? corp = _database.GetCorporateUser(buyer.Id);
                decimal? corpbuyer = corp.Credit + corp.Balance;
                if (buyer.Balance >= BidAmount && Auction.CurrentBuyer.Id != buyer.Id)
                {
                    if (Auction.CurrentBuyer != null && Auction.CurrentBuyer.Id != 0)
                    {
                        User currentBuyer = _database.GetUserById(Auction.CurrentBuyer.Id);

                        currentBuyer.Balance += Auction.CurrentPrice;
                        _database.UpdateUserBalance(currentBuyer.Id, currentBuyer.Balance);
                    }

                    buyer.Balance -= BidAmount;
                    _database.UpdateUserBalance(buyer.Id, buyer.Balance);

                    Auction.CurrentBuyer = buyer; 
                    Auction.CurrentPrice = BidAmount;
                    _database.UpdateAuction(Auction);
                }
                else if (buyer.IsCorp && corpbuyer >= BidAmount)
                {

                    if (Auction.CurrentBuyer != null && Auction.CurrentBuyer.Id != buyer.Id)
                    {
                        User currentBuyer = _database.GetUserById(Auction.CurrentBuyer.Id);

                        currentBuyer.Balance += Auction.CurrentPrice;
                        _database.UpdateUserBalance(currentBuyer.Id, currentBuyer.Balance);
                    }
                    Auction.CurrentBuyer = buyer;
                    Auction.CurrentPrice = BidAmount;
                    _database.UpdateAuction(Auction);

                    BidAmount -= buyer.Balance;
                    corp.Credit -= BidAmount;
                    corp.Balance = 0;

                    _database.UpdateCorporateUser(corp);
                }
                else
                {
                    Console.WriteLine("Insufficient balance to make this bid.");
                }
            }
            else
            {
                Console.WriteLine("Bid must be higher than the current price.");
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

        public void VehicleData(int VHId)
        {
            // Reset all visibility flags
            IsTruckVisible = false;
            IsBusVisible = false;
            IsHeavyVehicleVisible = false;
            IsNormalVHVisible = false;

            IsCommercialVisible = false;
            isCommercialVH = false;
            IsPrivateVisible = false;

            string VHType;
            int VHIdOut;

            // Get Vehicle Type and Id from the database
            _database.GetVehicleTypeAndId(VHId, out VHIdOut, out VHType);

            selectedVehicleType = VHType;

            if (VHType == "Truck")
            {
                Truck truck = _database.GetTruckById(VHIdOut);

                IsTruckVisible = true;
                IsHeavyVehicleVisible = true;

                EngineSize = truck.EngineSize;
                towBar = truck.TowHook;
                maxLoadCapacity = truck.MaxLoadCapacity;
                numberOfAxles = truck.NumberOfAxles;
                Height = truck.Height;
                Weight = truck.Weight;
                Length = truck.Length;
                LoadCapacity = truck.LoadCapacity;
            }
            else if (VHType == "Bus")
            {
                Bus bus = _database.GetABus(VHIdOut);

                IsHeavyVehicleVisible = true;
                IsBusVisible = true;

                EngineSize = bus.EngineSize;
                towBar = bus.TowHook;
                maxLoadCapacity = bus.MaxLoadCapacity;
                numberOfAxles = bus.NumberOfAxles;
                Height = bus.Height;
                Weight = bus.Weight;
                Length = bus.Length;
                NumberOfSeats = bus.NumberOfSeats;
                NumberOfSleepingPlaces = bus.NumberOfSleepingPlaces;
                HasToilet = bus.HasToilet;
            }
            else if (VHType == "PrivateVehicle")
            {
                PrivateVehicle privateVehicle = _database.GetPrivatVHById(VHIdOut);

                IsNormalVHVisible = true;
                IsPrivateVisible = true;

                EngineSize = privateVehicle.EngineSize;
                towBar = privateVehicle.TowHook;
                NumberOfSeats = privateVehicle.NumberOfSeats;
                TrunkDimensions = privateVehicle.TrunkDimensions;
                IsofixMount = privateVehicle.IsofixMount;
            }
            else if (VHType == "CommercialVehicle")
            {
                ComercialVehicle comercialVehicle = _database.GetComercialVehicleById(VHIdOut);

                IsNormalVHVisible = true;
                IsCommercialVisible = true;
                isCommercialVH = true;

                engineSize = comercialVehicle.EngineSize;
                TowBar = comercialVehicle.TowHook;
                numberOfSeats = comercialVehicle.NumberOfSeats;
                trunkDimensions = comercialVehicle.TrunkDimensions;
                RollCage = comercialVehicle.RollCage;
                LoadCapacity = comercialVehicle.LoadCapacity;
            }
        }
    }
}
