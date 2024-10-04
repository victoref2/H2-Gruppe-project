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
        EnergyClass = EnergyClassCalc(Convert.ToInt32(ageGroup), fuelType, KmL);
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
    public string EnergyClassCalc(int year, string fuelType, decimal KmL)
    {
        string energyClass = "";

        if (fuelType == "electric" || fuelType == "hydrogen")
        {
            energyClass = "Class A";
        }
        else if (fuelType == "diesel")
        {
            if (year < 2010)
            {
                if (KmL >= 23) energyClass = "Class A";
                else if (KmL >= 18) energyClass = "Class B";
                else if (KmL >= 13) energyClass = "Class C";
                else energyClass = "Class D";
            }
            else // After 2010
            {
                if (KmL >= 25) energyClass = "Class A";
                else if (KmL >= 20) energyClass = "Class B";
                else if (KmL >= 15) energyClass = "Class C";
                else energyClass = "Class D";
            }
        }
        else if (fuelType == "petrol" || fuelType == "benzin")
        {
            if (year < 2010)
            {
                if (KmL >= 18) energyClass = "Class A";
                else if (KmL >= 14) energyClass = "Class B";
                else if (KmL >= 10) energyClass = "Class C";
                else energyClass = "Class D";
            }
            else // After 2010
            {
                if (KmL >= 20) energyClass = "Class A";
                else if (KmL >= 16) energyClass = "Class B";
                else if (KmL >= 12) energyClass = "Class C";
                else energyClass = "Class D";
            }
        }
        return energyClass;
    }

    public override string ToString()
    {
        return $"Vehicle [ID: {Id}, Name: {Name}, Registration Number: {RegistrationNumber}, " +
               $"Kilometers: {KM}, Age Group: {AgeGroup}, Tow Hook: {(TowHook ? "Yes" : "No")}, " +
               $"Driver's Licence Class: {DriversLicenceClass}, Engine Size: {EngineSize}, " +
               $"Fuel Type: {FuelType}, Km/L: {KmL}, Energy Class: {EnergyClass}]";
    }
}
