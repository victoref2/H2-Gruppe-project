using H2_Gruppe_project.DatabaseClasses;
using H2_Gruppe_project.ViewModels;
using System;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes
{
    public class ViewModelVHToDatabase
    {
        private readonly MainWindowViewModel _mainViewModel;
        private readonly User _loggedInUser;
        private readonly Database _database;
        public ViewModelVHToDatabase(MainWindowViewModel mainViewModel, User loggedInUser, Database database)
        {
            _mainViewModel = mainViewModel;
            _loggedInUser = loggedInUser;
            _database = database;


        }
        public bool VHFromViewmodelToDatabase(Vehicle vehicle, string VHType,bool IsCommercialVH,int LoadCapacity, 
            int NumberOfAxels, decimal Height, decimal Weight, decimal Length, int MaxLoadCapacity, int NumberOfSeats,
            int NumberOfSleepingPlaces, bool HasToilet, string TrunkDimensions, bool RollCage, bool IsofixMount, 
            decimal StartingBid, DateTime ClosingDate)
        {
            if(VHType == "Truck"|| VHType == "Bus")
            {
                HeavyVehicle heavyVehicle = new(
                        id: 0,
                        name: vehicle.Name,
                        km: vehicle.Name,
                        registrationNumber: vehicle.RegistrationNumber,
                        ageGroup: vehicle.AgeGroup,
                        towHook: vehicle.TowHook,
                        driversLicenceClass: vehicle.DriversLicenceClass,
                        engineSize: vehicle.EngineSize,
                        kmL: vehicle.KmL,
                        fuelType: vehicle.FuelType,
                        energyClass: vehicle.EnergyClass,
                        maxLoadCapacity: MaxLoadCapacity,
                        numberOfAxles: NumberOfAxels
                        );

                if (VHType == "Truck")
                {
                    vehicle.DriversLicenceClass = vehicle.TowHook ? "CE" : "C";
                    heavyVehicle.DriversLicenceClass = vehicle.DriversLicenceClass;
                    Truck TVehicle = new(
                        id: 0,
                        name: vehicle.Name,
                        km: vehicle.KM,
                        registrationNumber: vehicle.RegistrationNumber,
                        ageGroup: vehicle.AgeGroup,
                        towHook: vehicle.TowHook,
                        driversLicenceClass: vehicle.DriversLicenceClass,
                        engineSize: vehicle.EngineSize,
                        kmL: vehicle.KmL,
                        fuelType: vehicle.FuelType,
                        energyClass: vehicle.EnergyClass,
                        maxLoadCapacity: MaxLoadCapacity,
                        numberOfAxles: NumberOfAxels,
                        height: Height,
                        weight: Weight,
                        length: Length,
                        loadCapacity: LoadCapacity
                    );
                    vehicle.Id = _database.AddTruckFlow(TVehicle,vehicle,heavyVehicle);
                }
                else if (VHType == "Bus")
                {
                    vehicle.DriversLicenceClass = vehicle.TowHook ? "DE" : "D";
                    heavyVehicle.DriversLicenceClass = vehicle.DriversLicenceClass;
                    Bus BVehicle = new(
                        id: 0,
                        name: vehicle.Name,
                        km: vehicle.KM,
                        registrationNumber: vehicle.RegistrationNumber,
                        ageGroup: vehicle.AgeGroup,
                        towHook: vehicle.TowHook,
                        driversLicenceClass: vehicle.DriversLicenceClass,
                        engineSize: vehicle.EngineSize,
                        kmL: vehicle.KmL,
                        fuelType: vehicle.FuelType,
                        energyClass: vehicle.EnergyClass,
                        maxLoadCapacity: MaxLoadCapacity,
                        numberOfAxles: NumberOfAxels,
                        height: Height,
                        weight: Weight,
                        length: Length,
                        numberOfSeats: NumberOfSeats,
                        numberOfSleepingPlaces: NumberOfSleepingPlaces,
                        hasToilet: HasToilet
                    );
                    vehicle.Id = _database.AddBusFlow(BVehicle, vehicle, heavyVehicle);
                }
            }
            else if (VHType == "Normal Vehicle")
            {
                vehicle.DriversLicenceClass = "B";

                NormalVehicle normalVehicle = new(
                    id: 0,
                    name: vehicle.Name,
                    km: vehicle.KM,
                    registrationNumber: vehicle.RegistrationNumber,
                    ageGroup: vehicle.AgeGroup,
                    towHook: vehicle.TowHook,
                    driversLicenceClass: vehicle.DriversLicenceClass,
                    engineSize: vehicle.EngineSize,
                    kmL: vehicle.KmL,
                    fuelType: vehicle.FuelType,
                    energyClass: vehicle.EnergyClass,
                    NumberOfSeats,
                    TrunkDimensions,
                    IsCommercialVH
                    );

                if (IsCommercialVH)
                {
                    if (LoadCapacity > 750)
                    {
                        vehicle.DriversLicenceClass = "BE";
                        normalVehicle.DriversLicenceClass = vehicle.DriversLicenceClass;
                    }

                    ComercialVehicle CVehicle = new(
                        id: 0,
                        name: vehicle.Name,
                        km: vehicle.KM,
                        registrationNumber: vehicle.RegistrationNumber,
                        ageGroup: vehicle.AgeGroup,
                        towHook: vehicle.TowHook,
                        driversLicenceClass: vehicle.DriversLicenceClass,
                        engineSize: vehicle.EngineSize,
                        kmL: vehicle.KmL,
                        fuelType: vehicle.FuelType,
                        energyClass: vehicle.EnergyClass,
                        numberOfSeats: NumberOfSeats,
                        trunkDimensions: TrunkDimensions,
                        isCommercial: IsCommercialVH,
                        rollCage: RollCage,
                        loadCapacity: LoadCapacity
                    );
                    vehicle.Id = _database.AddCVehicleFlow(CVehicle, vehicle, normalVehicle);
                }
                else
                {
                    PrivateVehicle PVehicle = new(
                        id: 0,
                        name: vehicle.Name,
                        km: vehicle.KM,
                        registrationNumber: vehicle.RegistrationNumber,
                        ageGroup: vehicle.AgeGroup,
                        towHook: vehicle.TowHook,
                        driversLicenceClass: vehicle.DriversLicenceClass,
                        engineSize: vehicle.EngineSize,
                        kmL: vehicle.KmL,
                        fuelType: vehicle.FuelType,
                        energyClass: vehicle.EnergyClass,
                        numberOfSeats: NumberOfSeats,
                        trunkDimensions: TrunkDimensions,
                        isCommercial: IsCommercialVH,
                        isofixMount: IsofixMount
                    );
                   vehicle.Id = _database.AddPVehicleFlow(PVehicle, vehicle, normalVehicle);
                }
            }
            Auction auction = new Auction(
                    id: 0,
                    vehicle: vehicle,
                    seller: _loggedInUser,
                    currentPrice: StartingBid,
                    closingDate: ClosingDate
                );

            _database.AddAuction(auction);

            return true;
        }
    }
}
