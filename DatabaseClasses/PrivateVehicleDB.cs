using System;
using System.Data;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public int AddPVehicleFlow(PrivateVehicle privateVehicle, Vehicle vehicle, NormalVehicle normalVehicle)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int vehicleId = AddVehicle(connection, transaction, vehicle);

                    int normalVehicleId = AddNormalVehicle(connection, transaction, vehicleId, normalVehicle);

                    AddPrivateVehicle(connection, transaction, normalVehicleId, privateVehicle);

                    transaction.Commit();

                    connection.Close();
                    return vehicleId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error during vehicle creation", ex);
                }
            }
        }

        private void AddPrivateVehicle(SqlConnection connection, SqlTransaction transaction, int normalVehicleId, PrivateVehicle privateVehicle)
        {
            using (SqlCommand cmd = new SqlCommand("AddPrivateVehicles", connection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NormalVehiclesId", normalVehicleId);
                cmd.Parameters.AddWithValue("@IsofixMount", privateVehicle.IsofixMount);

                cmd.ExecuteNonQuery();
            }
        }

        public PrivateVehicle GetPrivateVehicleById(int vehicleId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT v.*, nv.NormalVehicleId, nv.NumberOfSeats, nv.TrunkDimensions, nv.IsCommercial, pv.IsofixMount
                    FROM Vehicles v
                    INNER JOIN NormalVehicles nv ON v.VehicleId = nv.VehicleId
                    INNER JOIN PrivateVehicles pv ON nv.NormalVehicleId = pv.NormalVehicleId
                    WHERE v.VehicleId = @VehicleId;";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@VehicleId", vehicleId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Ensure ID is treated as an integer
                        return new PrivateVehicle(
                            id: Convert.ToInt32(reader["VehicleId"]),  // Use int for the ID
                            name: reader["Name"].ToString(),
                            km: reader["KM"].ToString(),
                            registrationNumber: reader["RegistrationNumber"].ToString(),
                            ageGroup: reader["AgeGroup"].ToString(),
                            towHook: Convert.ToBoolean(reader["TowHook"]),
                            driversLicenceClass: reader["DriversLicenceClass"].ToString(),
                            engineSize: reader["EngineSize"].ToString() + "L",
                            kmL: Convert.ToDecimal(reader["KmL"]),
                            fuelType: reader["FuelType"].ToString(),
                            energyClass: reader["EnergyClass"].ToString(),
                            numberOfSeats: Convert.ToInt32(reader["NumberOfSeats"]),
                            trunkDimensions: reader["TrunkDimensions"].ToString(),
                            isCommercial: Convert.ToBoolean(reader["IsCommercial"]),
                            isofixMount: Convert.ToBoolean(reader["IsofixMount"])
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
