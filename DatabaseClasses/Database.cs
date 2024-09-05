using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public string ConnectionString { get; set; }

        public static Database Instance { get; private set; } = new Database();
    }
}
