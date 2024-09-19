using System;

namespace H2_Gruppe_project.Classes
{
    public class Truck
    {
        public double Height { get; private set; }
        public double Weight { get; private set; }
        public double Length { get; private set; }
        public double LoadCapacity { get; private set; }
        public string DriversLicenseClass { get; private set; }
        public double EngineSize { get; private set; }
        public bool TowHook { get; private set; }
        public Truck(double height, double weight, double length, double loadCapacity, double engineSize, bool towHook)
        {
            Height = height;
            Weight = weight;
            Length = length;
            LoadCapacity = loadCapacity;
            TowHook = towHook;
            if (engineSize < 4.2 || engineSize > 15.0)
            {
                throw new ArgumentOutOfRangeException("EngineSize", "Engine size must be between 4.2 and 15.0 liters.");
            }
            EngineSize = engineSize;

            DriversLicenseClass = towHook ? "CE" : "C"; // If it has a tow hook, it requires a CE license
        }
        public override string ToString()
        {
            return $"Truck [Height: {Height}m, Weight: {Weight}kg, Length: {Length}m, " +
                   $"Load Capacity: {LoadCapacity}kg, Engine Size: {EngineSize}L, " +
                   $"Tow Hook: {(TowHook ? "Yes" : "No")}, License Required: {DriversLicenseClass}]";
        }
    }
}
