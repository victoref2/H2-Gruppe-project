using System;

namespace H2_Gruppe_project.Classes
{
    public class Truck : HeavyVehicle
    {
        public int TruckId { get; private set; }
        public decimal Height { get; private set; }
        public decimal Weight { get; private set; }
        public decimal Length { get; private set; }
        public decimal LoadCapacity { get; private set; }

        public Truck(string id, string name, string km, string regristrationNumber, string ageGroup, bool towHook, string driversLicenceClass,
            string engineSize, decimal kmL, string fuelType, string energyClass, int maxLoadCapacity, int numberOfAxles,
            decimal height, decimal weight, decimal length, decimal loadCapacity)
            : base(id, name, km, regristrationNumber, ageGroup, towHook, driversLicenceClass, engineSize, kmL, fuelType, energyClass, maxLoadCapacity, numberOfAxles)
        {
            if (decimal.Parse(engineSize) < 4.2m || decimal.Parse(engineSize) > 15.0m)
            {
                throw new ArgumentOutOfRangeException(nameof(engineSize), "Engine size must be between 4.2L and 15.0L.");
            }

            Height = height;
            Weight = weight;
            Length = length;
            LoadCapacity = loadCapacity;

            // Handle driver's license type based on tow hook.
            if (towHook)
            {
                DriversLicenceClass = "CE";
            }
            else
            {
                DriversLicenceClass = "C";
            }
        }

        public override string ToString()
        {
            return base.ToString() + $", Height: {Height} meters, Weight: {Weight} kg, Length: {Length} meters, Load Capacity: {LoadCapacity} tons";
        }
    }
}
