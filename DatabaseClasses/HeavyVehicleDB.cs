using H2_Gruppe_project.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        private int AddHeavyVehicle(SqlConnection connection, SqlTransaction transaction, int vehicleId, HeavyVehicle heavyVehicle)
        {
            using (SqlCommand cmd = new SqlCommand("AddHeavyVehicle", connection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@VehicleId",vehicleId);
                cmd.Parameters.AddWithValue("@MaxLoadCapacity", heavyVehicle.MaxLoadCapacity); 
                cmd.Parameters.AddWithValue("@NumberOfAxles", heavyVehicle.NumberOfAxles);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
