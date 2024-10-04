using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using H2_Gruppe_project.DatabaseClasses;
using System;
using H2_Gruppe_project.Classes;
using Avalonia.Controls;
using System.Collections.Generic;

namespace H2_Gruppe_project.ViewModels
{
    public partial class AddVHViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainViewModel;
        private readonly User _loggedInUser;
        private readonly Database _database;

        

        public AddVHViewModel(MainWindowViewModel mainViewModel, User loggedInUser, Database database)
        {
            _mainViewModel = mainViewModel;
            _loggedInUser = loggedInUser;
            _database = database;


        }

        // Common Properties
        [ObservableProperty]
        private string vehicleName;

        [ObservableProperty]
        private string mileage;

        [ObservableProperty]
        private string registrationNumber;

        [ObservableProperty]
        private string fuelType;

        [ObservableProperty]
        private decimal startingBid;

        [ObservableProperty]
        private DateTimeOffset closingDate = DateTimeOffset.Now;

        [ObservableProperty]
        private string selectedVehicleType;

        [ObservableProperty]
        private DateTimeOffset ageGroup = DateTimeOffset.Now;

        // Additional Common Properties
        [ObservableProperty]
        private string driversLicenseClass;

        [ObservableProperty]
        private decimal newPrice;

        [ObservableProperty]
        private string energyClass;

        // Vehicle Type Specific Properties
        [ObservableProperty]
        private decimal height;

        [ObservableProperty]
        private decimal length;

        [ObservableProperty]
        private decimal weight;

        [ObservableProperty]
        private string engineSize;

        [ObservableProperty]
        private decimal _KmL;

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
        private int loadCapacity;

        [ObservableProperty]
        private int maxLoadCapacity;

        [ObservableProperty]
        private int numberOfAxles;

        [ObservableProperty]
        private int numberOfSleepingPlaces;

        [ObservableProperty]
        private bool hasToilet;

        [ObservableProperty]
        private string statusMessage;

        // Validation State Properties (Optional but useful for providing specific feedback)
        [ObservableProperty]
        private string registrationNumberErrorMessage;

        [ObservableProperty]
        private string mileageErrorMessage;

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
        
        [ObservableProperty]
        private bool isofixMount;


        // Command for going back to dashboard
        [RelayCommand]
        public void GoBackCancel()
        {
            _mainViewModel.SwitchViewModel(new DashboardViewModel(_mainViewModel, _loggedInUser, _database));
        }

        partial void OnIsCommercialVHChanged(bool value)
        {
            // Set visibility based on the checkbox
            IsCommercialVisible = value;
            IsPrivateVisible = !value;
        }


        public List<string> VehicleTypes { get; } = new List<string> { "Truck", "Bus", "Normal Vehicle" };

        partial void OnSelectedVehicleTypeChanged(string value)
        {
            // Reset all visibilities
            IsTruckVisible = false;
            IsBusVisible = false;
            IsHeavyVehicleVisible = false;
            IsNormalVHVisible = false;

            IsCommercialVisible = false;
            IsCommercialVH = false;
            IsPrivateVisible = false;

            if (value == "Truck")
            {
                IsTruckVisible = true;
                IsHeavyVehicleVisible = true;
            }
            else if (value == "Bus")
            {
                IsHeavyVehicleVisible = true;
                IsBusVisible = true;

            }
            else if (value == "Normal Vehicle")
            {
                IsNormalVHVisible = true;
                IsPrivateVisible = true;
            }
        }

        [RelayCommand]
        public async Task CreateAuctionAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(VehicleName) ||
                    string.IsNullOrWhiteSpace(Mileage) ||
                    string.IsNullOrWhiteSpace(RegistrationNumber))
                {
                    StatusMessage = "All fields are required.";
                    return;
                }

                if (SelectedVehicleType == null)
                {
                    StatusMessage = "Please select a vehicle type.";
                    return;
                }


                Vehicle vehicle = null;
                string name = VehicleName;
                string km = Mileage;
                string registrationNumber = RegistrationNumber;

                // Convert ageGroup from string to int
                string ageGroup = AgeGroup.Year.ToString();
                int ageGroupInt;
                if (!int.TryParse(ageGroup, out ageGroupInt))
                {
                    // Handle invalid conversion, if necessary
                    throw new FormatException("Invalid Age Group format");
                }

                decimal kmL = _KmL;
                bool towHook = TowBar;
                string driversLicenceClass = "";
                string fuelType = FuelType;
                Vehicle vehicle1 = new(0,VehicleName, Mileage, RegistrationNumber, AgeGroup.Year.ToString(),TowBar,"",EngineSize,kmL,fuelType,"");
                string energyclass = vehicle1.EnergyClassCalc(ageGroupInt, fuelType,kmL);

                if (SelectedVehicleType == "Truck")
                {
                    driversLicenceClass = towHook ? "CE" : "C";

                    vehicle = new Truck(
                        id: 0,
                        name: name,
                        km: km,
                        registrationNumber: registrationNumber,
                        ageGroup: ageGroup,
                        towHook: towHook,
                        driversLicenceClass: driversLicenceClass,
                        engineSize: EngineSize,
                        kmL: kmL,
                        fuelType: fuelType,
                        energyClass: energyclass,
                        maxLoadCapacity: MaxLoadCapacity,
                        numberOfAxles: NumberOfAxles,
                        height: Height,
                        weight: Weight,
                        length: Length,
                        loadCapacity: LoadCapacity
                    );
                }
                else if (SelectedVehicleType == "Bus")
                {
                    driversLicenceClass = towHook ? "DE" : "D";

                    vehicle = new Bus(
                        id: 0,
                        name: name,
                        km: km,
                        registrationNumber: registrationNumber,
                        ageGroup: ageGroup,
                        towHook: towHook,
                        driversLicenceClass: driversLicenceClass,
                        engineSize: EngineSize,
                        kmL: kmL,
                        fuelType: fuelType,
                        energyClass: null,
                        maxLoadCapacity: MaxLoadCapacity,
                        numberOfAxles: NumberOfAxles,
                        height: Height,
                        weight: Weight,
                        length: Length,
                        numberOfSeats: NumberOfSeats,
                        numberOfSleepingPlaces: NumberOfSleepingPlaces,
                        hasToilet: HasToilet
                    );
                }
                else if (SelectedVehicleType == "Normal Vehicle")
                {
                    bool isCommercial = IsCommercialVH;
                    driversLicenceClass = "B";

                    if (isCommercial)
                    {
                        if (LoadCapacity > 750)
                        {
                            driversLicenceClass = "BE";
                        }

                        vehicle = new ComercialVehicle(
                            id: 0,
                            name: name,
                            km: km,
                            registrationNumber: registrationNumber,
                            ageGroup: ageGroup,
                            towHook: towHook,
                            driversLicenceClass: driversLicenceClass,
                            engineSize: EngineSize,
                            kmL: kmL,
                            fuelType: fuelType,
                            energyClass: null,
                            numberOfSeats: NumberOfSeats,
                            trunkDimensions: TrunkDimensions,
                            isCommercial: isCommercial,
                            rollCage: RollCage,
                            loadCapacity: LoadCapacity
                        );
                    }
                    else
                    {
                        vehicle = new PrivateVehicle(
                            id: 0,
                            name: name,
                            km: km,
                            registrationNumber: registrationNumber,
                            ageGroup: ageGroup,
                            towHook: towHook,
                            driversLicenceClass: driversLicenceClass,
                            engineSize: EngineSize,
                            kmL: kmL,
                            fuelType: fuelType,
                            energyClass: null,
                            numberOfSeats: NumberOfSeats,
                            trunkDimensions: TrunkDimensions,
                            isCommercial: isCommercial,
                            isofixMount: IsofixMount
                        );
                    }
                }

                if (vehicle == null)
                {
                    StatusMessage = "Failed to create vehicle.";
                    return;
                }

                await Task.Run(() =>
                {
                    _database.AddVehicle(vehicle);
                    Auction auction = new Auction(
                        id: 0,
                        vehicle: vehicle,
                        seller: _loggedInUser,
                        currentPrice: StartingBid,
                        closingDate: ClosingDate.DateTime
                    );

                    _database.AddAuction(auction);
                });

                StatusMessage = "Auction created successfully.";

                _mainViewModel.SwitchViewModel(new DashboardViewModel(_mainViewModel, _loggedInUser, _database));
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error creating auction: {ex.Message}";
            }
        }
    }
}