﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes
{
    public class NormalVehicle : Vehicle
    {
        public int NormalVehicleId {get; set; }
        public int NumberOfSeats { get; private set; }
        public string TrunkDimensions { get; private set; } // e.g., "1.5m x 1m x 0.8m"

        public NormalVehicle(string id, string name, string km, string regristrationNumber, string ageGroup, bool towHook,
            string driversLicenceClass, string engineSize, decimal kmL, string fuelType, string energyClass,
            int numberOfSeats, string trunkDimensions, bool isCommercial)
            : base(id, name, km, regristrationNumber, ageGroup, towHook, driversLicenceClass, engineSize, kmL, fuelType, energyClass)
        {

            if (isCommercial && towHook)
            {
                DriversLicenceClass = "BE";
            }
            else
            {
                DriversLicenceClass = "B";
            }

            NumberOfSeats = numberOfSeats;
            TrunkDimensions = trunkDimensions;
            ValidateEngineSize(engineSize);
        }

        private void ValidateEngineSize(string engineSize)
        {
            if (decimal.TryParse(engineSize.TrimEnd('L', 'l'), out decimal engineCapacity))
            {
                if (engineCapacity < 0.7m || engineCapacity > 10.0m)
                {
                    throw new ArgumentOutOfRangeException(nameof(engineSize), "Engine size must be between 0.7 and 10.0 liters.");
                }
            }
            else
            {
                throw new ArgumentException("Invalid engine size format. It should be a decimal followed by 'L'.", nameof(engineSize));
            }
        }
        public override string ToString()
        {
            return base.ToString() + $", Number of Seats: {NumberOfSeats}, Trunk Dimensions: {TrunkDimensions}";
        }
    }
}

