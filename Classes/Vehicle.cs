using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes
{
    internal class Vehicle
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string KM { get; private set; }
        public string RegristrationNumber {  get; private set; }
        public string AgeGroup { get; private set; }
        public bool TowHook { get; set; }
        public string Drives {  get; private set; }
    }
}
