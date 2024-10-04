using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes
{
    public class ComercialVehicle : NormalVehicle
    {
        public bool RollCage { get; set; } 
        public int LoadCapacity { get; set; } 

        public ComercialVehicle(string id, string name, string km, string registrationNumber, string ageGroup, bool towHook,
            string driversLicenceClass, string engineSize, decimal kmL, string fuelType, string energyClass,
            int numberOfSeats, string trunkDimensions, bool isCommercial, bool rollCage, int loadCapacity)
            : base(id, name, km, registrationNumber, ageGroup, towHook, driversLicenceClass, engineSize, kmL, fuelType, energyClass,
                   numberOfSeats, trunkDimensions, isCommercial)
        {
            RollCage = rollCage;

            if (loadCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(loadCapacity), "Load capacity must be a positive number.");
            }

            LoadCapacity = loadCapacity;
        }

        public override string ToString()
        {
            return base.ToString() + $", Roll Cage: {(RollCage ? "Yes" : "No")}, Load Capacity: {LoadCapacity} kg";
        }
    }
}
