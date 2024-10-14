using System;
using System.Data;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public int AddCVehicleFlow(ComercialVehicle comercialVehicle, Vehicle vehicle, NormalVehicle normalVehicle)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int vehicleId = AddVehicle(connection, transaction, vehicle);

                    int normalVehicleId = AddNormalVehicle(connection, transaction, vehicleId, normalVehicle);

                    AddComercialVehicle(connection, transaction, normalVehicleId, comercialVehicle);

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
        public void AddComercialVehicle(SqlConnection connection, SqlTransaction transaction, int normalVehicleId, ComercialVehicle comercialVehicle)
        {
            using (SqlCommand cmd = new SqlCommand("AddCommercialVehicles", connection, transaction)) 
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NormalVehiclesId", comercialVehicle.NormalVehicleId);
                cmd.Parameters.AddWithValue("@RollCage", comercialVehicle.RollCage);
                cmd.Parameters.AddWithValue("@LoadCapacity", comercialVehicle.LoadCapacity);

                cmd.ExecuteNonQuery();
            }
        }

        public ComercialVehicle GetComercialVehicleById(int vehicleId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT v.*, nv.NormalVehicleId, nv.NumberOfSeats, nv.TrunkDimensions, nv.IsCommercial, cv.RollCage, cv.LoadCapacity
                    FROM Vehicles v
                    INNER JOIN NormalVehicles nv ON v.VehicleId = nv.VehicleId
                    INNER JOIN ComercialVehicles cv ON nv.NormalVehicleId = cv.NormalVehicleId
                    WHERE v.VehicleId = @VehicleId;";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@VehicleId", vehicleId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ComercialVehicle(
                            id: Convert.ToInt32(reader["VehicleId"]),
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
                            rollCage: Convert.ToBoolean(reader["RollCage"]),
                            loadCapacity: Convert.ToInt32(reader["LoadCapacity"])
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
