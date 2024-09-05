using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes;

public class HeavyVehicle : Vehicle
{

    public int MaxLoadCapacity { get; private set; }
    public int NumberOfAxles { get; private set; }

    public HeavyVehicle(string id, string name, string km, string regristrationNumber, string ageGroup, bool towHook, string driversLicenceClass,
        string engineSize, decimal kmL, string fuelType, string energyClass, int maxLoadCapacity, int numberOfAxles)
        : base(id, name, km, regristrationNumber, ageGroup, towHook, driversLicenceClass, engineSize, kmL, fuelType, energyClass)
    {
        MaxLoadCapacity = maxLoadCapacity;
        NumberOfAxles = numberOfAxles;
    }

    public override string ToString()
    {
        return base.ToString() + $", Max Load Capacity: {MaxLoadCapacity} kg, Number of Axles: {NumberOfAxles}";
    }
}
