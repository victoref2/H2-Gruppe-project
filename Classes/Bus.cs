using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes;

public class Bus : HeavyVehicle
{
    public decimal Height { get; private set; }
    public decimal Weight { get; private set; }
    public decimal Length { get; private set; }
    public int NumberOfSeats { get; private set; }
    public int NumberOfSleepingPlaces { get; private set; }
    public bool HasToilet { get; private set; }

    public Bus(string id, string name, string km, string regristrationNumber, string ageGroup, bool towHook,
               string engineSize, decimal kmL, string fuelType, string energyClass,
               int maxLoadCapacity, int numberOfAxles,
               decimal height, decimal weight, decimal length,
               int numberOfSeats, int numberOfSleepingPlaces, bool hasToilet)
        : base(id, name, km, regristrationNumber, ageGroup, towHook, "D", engineSize, kmL, fuelType, energyClass, maxLoadCapacity, numberOfAxles)
    {
        Height = height;
        Weight = weight;
        Length = length;
        NumberOfSeats = numberOfSeats;
        NumberOfSleepingPlaces = numberOfSleepingPlaces;
        HasToilet = hasToilet;

        if (TowHook)
        {
            DriversLicenceClass = "DE";
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
        return base.ToString() + $", Height: {Height}m, Weight: {Weight}kg, Length: {Length}m, " +
               $"Seats: {NumberOfSeats}, Sleeping Places: {NumberOfSleepingPlaces}, Toilet: {(HasToilet ? "Yes" : "No")}";
    }
}