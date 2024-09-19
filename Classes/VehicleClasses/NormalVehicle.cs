using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace H2_Gruppe_project.Classes
{
    public class NormalVehicle : Vehicle
    {
        public int NumberOfSeats { get; private set; }
        public string TrunkDimensions { get; private set; } // e.g., "1.5m x 1m x 0.8m"

        public NormalVehicle(string id, string name, string km, string regristrationNumber, string ageGroup, bool towHook,
            string driversLicenceClass, string engineSize, decimal kmL, string fuelType, string energyClass,
            int numberOfSeats, string trunkDimensions, bool isCommercial)
            : base(id, name, km, regristrationNumber, ageGroup, towHook, driversLicenceClass, engineSize, kmL, fuelType, energyClass)
        {
            if (decimal.Parse(engineSize) < 0.7m || decimal.Parse(engineSize) > 10.0m)
            {
                throw new ArgumentOutOfRangeException(nameof(engineSize), "Engine size must be between 0.7L and 10.0L.");
            }

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
        }

        public override string ToString()
        {
            return base.ToString() + $", Number of Seats: {NumberOfSeats}, Trunk Dimensions: {TrunkDimensions}";
        }
    }
}

