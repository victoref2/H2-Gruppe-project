using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes;

public class Bus : HeavyVehicle
{
    public int BusId { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public decimal Length { get; set; }
    public int NumberOfSeats { get; set; }
    public int NumberOfSleepingPlaces { get;set; }
    public bool HasToilet { get; set; }

    public Bus(string id, string name, string km, string registrationNumber, string ageGroup, bool towHook, string driversLicenceClass,
               string engineSize, decimal kmL, string fuelType, string energyClass,
               int maxLoadCapacity, int numberOfAxles,
               decimal height, decimal weight, decimal length,
               int numberOfSeats, int numberOfSleepingPlaces, bool hasToilet)
        : base(id, name, km, registrationNumber, ageGroup, towHook, driversLicenceClass, engineSize, kmL, fuelType, energyClass, maxLoadCapacity, numberOfAxles)
    {
        Height = height;
        Weight = weight;
        Length = length;
        NumberOfSeats = numberOfSeats;
        NumberOfSleepingPlaces = numberOfSleepingPlaces;
        HasToilet = hasToilet;

        if (TowHook && driversLicenceClass == "D")
        {
            DriversLicenceClass = "DE";
        }

        ValidateEngineSize(engineSize);
    }

        private static bool ValidateRegistrationNumber(string regNumber)
    {
        return regNumber.Length == 7 && char.IsLetter(regNumber[0]) && char.IsLetter(regNumber[1]) &&
            int.TryParse(regNumber.Substring(2), out _);
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