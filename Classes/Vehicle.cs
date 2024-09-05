using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes;

public class Vehicle
{
    //change privacy and what it is as needed
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string KM { get; private set; }
    public string RegristrationNumber { get; private set; }
    public string AgeGroup { get; private set; }
    public bool TowHook { get; set; }
    public string DriversLicenceClass { get; protected set; }
    public string EngineSize { get; private set; }
    public decimal KmL { get; private set; }
    public string FuelType { get; private set; }
    public string EnergyClass { get; private set; }
    public Vehicle(string id, string name, string km, string regristrationNumber, string ageGroup, bool towHook, string driversLicenceClass,
        string engineSize, decimal kmL, string fuelType, string energyClass)
    {
        Id = id;
        Name = name;
        KM = km;
        RegristrationNumber = regristrationNumber;
        AgeGroup = ageGroup;
        TowHook = towHook;
        DriversLicenceClass = driversLicenceClass;
        EngineSize = engineSize;
        KmL = kmL;
        FuelType = fuelType;
        EnergyClass = energyClass;
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
        return $"Vehicle [ID: {Id}, Name: {Name}, Registration Number: {RegristrationNumber}, " +
               $"Kilometers: {KM}, Age Group: {AgeGroup}, Tow Hook: {(TowHook ? "Yes" : "No")}, " +
               $"Driver's Licence Class: {DriversLicenceClass}, Engine Size: {EngineSize}, " +
               $"Fuel Type: {FuelType}, Km/L: {KmL}, Energy Class: {EnergyClass}]";
    }
}
