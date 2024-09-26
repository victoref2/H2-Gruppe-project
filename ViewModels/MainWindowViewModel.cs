using CommunityToolkit.Mvvm.ComponentModel;

namespace H2_Gruppe_project.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public MainWindowViewModel()
        {
            // Initialize with the Main Menu View
            CurrentViewModel = new MainMenuViewModel(this);
        }
    }
}


/*public string Greeting { get; }

        public MainWindowViewModel()
        {
            Greeting = TestVehicleAndBusDatabaseOperations();
        }

        private string TestVehicleAndBusDatabaseOperations()
        {
            try
            {
                Database db = new Database();

                Vehicle vehicle = new Vehicle(
                    id: null, // 
                    name: "Generic Vehicle",
                    km: "25000",
                    regristrationNumber: "GV12345",
                    ageGroup: "2022",
                    towHook: false,
                    driversLicenceClass: "B",
                    engineSize: "2.0",
                    kmL: 18.0m,
                    fuelType: "petrol",
                    energyClass: "Class A"
                );

                db.AddVehicle(vehicle);

                Vehicle retrievedVehicle = db.GetVehicle(int.Parse(vehicle.Id));
                string result = "Vehicle added to database.\n";
                if (retrievedVehicle != null)
                {
                    result += retrievedVehicle.ToString() + "\n";
                }
                else
                {
                    result += "Vehicle not found.\n";
                }

                Bus bus = new Bus(
                    id: null,
                    name: "Mercedes Benz Bus",
                    km: "50000",
                    regristrationNumber: "XY12345",
                    ageGroup: "2020",
                    towHook: true,
                    engineSize: "10.5L",
                    kmL: 14.0m,
                    fuelType: "diesel",
                    energyClass: "Class A",
                    maxLoadCapacity: 30000,
                    numberOfAxles: 2,
                    height: 4.5m,
                    weight: 15000m,
                    length: 12.0m,
                    numberOfSeats: 50,
                    numberOfSleepingPlaces: 20,
                    hasToilet: true
                );

                db.AddBus(bus);

                Bus retrievedBus = db.GetBusById(bus.BusId);
                result += "Bus added to database.\n";
                if (retrievedBus != null)
                {
                    result += retrievedBus.ToString() + "\n";
                }
                else
                {
                    result += "Bus not found.\n";
                }

                db.DeleteVehicle(int.Parse(vehicle.Id));
                db.DeleteBus(bus.BusId);

                result += "Vehicle and Bus deleted from database.";

                return result;
            }
            catch (Exception ex)
            {
                return $"Error during database operations: {ex.Message}";
            }
        }*/