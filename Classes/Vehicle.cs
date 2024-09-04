using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes
{
    internal class Vehicle
    {
        //change privacy and what it is as needed
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string KM { get; private set; }
        public string RegristrationNumber {  get; private set; }
        public string AgeGroup { get; private set; }
        public bool TowHook { get; set; }
        public string DrivesLicenceClass {  get; private set; }
        public string EngienSize { get; private set; }
        public string KmL {  get; private set; }
        public string FuelType {  get; private set; }
        public string EnergyClass {  get; private set; }
    }
}
