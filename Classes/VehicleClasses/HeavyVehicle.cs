﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes;

public class HeavyVehicle : Vehicle
{

    public int MaxLoadCapacity { get; set; }
    public int NumberOfAxles { get; set; }

    public HeavyVehicle(int id, string name, string km, string registrationNumber, string ageGroup, bool towHook, string driversLicenceClass,
        string engineSize, decimal kmL, string fuelType, string energyClass, int maxLoadCapacity, int numberOfAxles)
        : base(id, name, km, registrationNumber, ageGroup, towHook, driversLicenceClass, engineSize, kmL, fuelType, energyClass)
    {
        MaxLoadCapacity = maxLoadCapacity;
        NumberOfAxles = numberOfAxles;
    }

    public override string ToString()
    {
        return base.ToString() + $", Max Load Capacity: {MaxLoadCapacity} kg, Number of Axles: {NumberOfAxles}";
    }
}
