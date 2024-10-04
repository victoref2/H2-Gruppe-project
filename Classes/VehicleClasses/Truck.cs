using System;

namespace H2_Gruppe_project.Classes
{
    public class Truck : HeavyVehicle
    {
        public int TruckId { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal LoadCapacity { get; set; }

        public Truck(int id, string name, string km, string registrationNumber, string ageGroup, bool towHook, string driversLicenceClass,
            string engineSize, decimal kmL, string fuelType, string energyClass, int maxLoadCapacity, int numberOfAxles,
            decimal height, decimal weight, decimal length, decimal loadCapacity)
            : base(id, name, km, registrationNumber, ageGroup, towHook, driversLicenceClass, engineSize, kmL, fuelType, energyClass, maxLoadCapacity, numberOfAxles)
        {
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

            ValidateEngineSize(engineSize);
        }

        private void ValidateEngineSize(string engineSize)
        {
            if (decimal.TryParse(engineSize.TrimEnd('L', 'l'), out decimal engineCapacity))
            {
                if (engineCapacity < 4.2m || engineCapacity > 15.0m)
                {
                    throw new ArgumentOutOfRangeException(nameof(engineSize), "Engine size must be between 4.2 and 15.0 liters.");
                }
            }
            else
            {
                throw new ArgumentException("Invalid engine size format. It should be a decimal followed by 'L'.", nameof(engineSize));
            }
        }

        public override string ToString()
        {
            return base.ToString() + $", Height: {Height} meters, Weight: {Weight} kg, Length: {Length} meters, Load Capacity: {LoadCapacity} tons";
        }
    }
}
