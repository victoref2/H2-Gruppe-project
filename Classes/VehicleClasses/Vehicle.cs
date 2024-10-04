using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes;

public class Vehicle
{
    //change privacy and what it is as needed
    public int Id { get; set; }
    public string Name { get; set; }
    public string KM { get;  set; }
    public string RegistrationNumber  { get; set; }
    public string AgeGroup { get; set; }
    public bool TowHook { get; set; }
    public string DriversLicenceClass { get; protected set; }
    public string EngineSize { get;  set; }
    public decimal KmL { get;  set; }
    public string FuelType { get;  set; }
    public string EnergyClass { get;  set; }
    public Vehicle(int id, string name, string km, string registrationNumber, string ageGroup, bool towHook, string driversLicenceClass,
        string engineSize, decimal kmL, string fuelType, string energyClass)
    {
        Id = id;
        Name = name;
        KM = km;
        RegistrationNumber = ValidateRegistrationNumber(registrationNumber);
        AgeGroup = ageGroup;
        TowHook = towHook;
        DriversLicenceClass = driversLicenceClass;
        EngineSize = engineSize;
        KmL = kmL;
        FuelType = fuelType;
        EnergyClass = energyClass;
    }

    private string ValidateRegistrationNumber(string regNumber)
    {
        if (regNumber.Length == 7 &&
            char.IsLetter(regNumber[0]) &&
            char.IsLetter(regNumber[1]) &&
            int.TryParse(regNumber.Substring(2), out _))
        {
            return regNumber;
        }
        throw new ArgumentException("Invalid registration number format. Must be a danish RegistrationNumber");
    }
    public void EnergyClassCalc(Vehicle vehicle)
    {
        int year = int.Parse(AgeGroup);
        if (vehicle.FuelType == "electric" || vehicle.FuelType == "hydrogen")
        {
            EnergyClass = "Class A";
        }
        else if (vehicle.FuelType == "diesel")
        {
            if (year < 2010)
            {
                if (KmL >= 23) EnergyClass = "Class A";
                else if (KmL >= 18) EnergyClass = "Class B";
                else if (KmL >= 13) EnergyClass = "Class C";
                else EnergyClass = "Class D";
            }
            else // After 2010
            {
                if (KmL >= 25) EnergyClass = "Class A";
                else if (KmL >= 20) EnergyClass = "Class B";
                else if (KmL >= 15) EnergyClass = "Class C";
                else EnergyClass = "Class D";
            }
        }
        else if (vehicle.FuelType == "petrol" || vehicle.FuelType == "benzin")
        {
            if (year < 2010)
            {
                if (KmL >= 18) EnergyClass = "Class A";
                else if (KmL >= 14) EnergyClass = "Class B";
                else if (KmL >= 10) EnergyClass = "Class C";
                else EnergyClass = "Class D";
            }
            else // After 2010
            {
                if (KmL >= 20) EnergyClass = "Class A";
                else if (KmL >= 16) EnergyClass = "Class B";
                else if (KmL >= 12) EnergyClass = "Class C";
                else EnergyClass = "Class D";
            }
        }
    }
    public override string ToString()
    {
        return $"Vehicle [ID: {Id}, Name: {Name}, Registration Number: {RegistrationNumber}, " +
               $"Kilometers: {KM}, Age Group: {AgeGroup}, Tow Hook: {(TowHook ? "Yes" : "No")}, " +
               $"Driver's Licence Class: {DriversLicenceClass}, Engine Size: {EngineSize}, " +
               $"Fuel Type: {FuelType}, Km/L: {KmL}, Energy Class: {EnergyClass}]";
    }
}
