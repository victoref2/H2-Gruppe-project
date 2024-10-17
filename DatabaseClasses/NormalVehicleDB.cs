using System;
using System.Data;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        private int AddNormalVehicle(SqlConnection connection, SqlTransaction transaction, int vehicleId, NormalVehicle normalVehicle)
        {
            using (SqlCommand cmd = new SqlCommand("AddNormalVehicles", connection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@VehicleId", vehicleId);
                cmd.Parameters.AddWithValue("@NumberOfSeats", normalVehicle.NumberOfSeats);
                cmd.Parameters.AddWithValue("@TrunkDimensions", normalVehicle.TrunkDimensions);
                cmd.Parameters.AddWithValue("@IsCommercial", normalVehicle.IsCommercial);

                return Convert.ToInt32(cmd.ExecuteNonQuery());
            }
        }
    }
}
