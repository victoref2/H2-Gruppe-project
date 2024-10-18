using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;
using System.Collections.Generic;

namespace H2_Gruppe_project.ViewModels
{
    public partial class AuctionSellerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        // Auction data
        public Auction Auction { get; set; }

        private readonly User _loggedInUser;
        private readonly Database _database;

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
            VehicleData(Auction.Vehicle.Id);
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
