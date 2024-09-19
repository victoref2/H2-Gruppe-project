using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes
{
    public class PrivateVehicle : NormalVehicle
    {
        public bool IsofixMount { get; private set; }

        public PrivateVehicle(string id, string name, string km, string regristrationNumber, string ageGroup, bool towHook,
            string driversLicenceClass, string engineSize, decimal kmL, string fuelType, string energyClass,
            int numberOfSeats, string trunkDimensions, bool isCommercial, bool isofixMount)
            : base(id, name, km, regristrationNumber, ageGroup, towHook, driversLicenceClass, engineSize, kmL, fuelType, energyClass,
                   numberOfSeats, trunkDimensions, isCommercial)
        {
            IsofixMount = isofixMount; 
        }

        public override string ToString()
        {
            return base.ToString() + $", Isofix Mount: {(IsofixMount ? "Yes" : "No")}";
        }
    }
}

