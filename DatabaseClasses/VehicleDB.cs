using System;
using System.Data;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;
using HarfBuzzSharp;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        // Create - Add Vehicle
        private int AddVehicle(SqlConnection connection, SqlTransaction transaction, Vehicle vehicle)
        {
            using (SqlCommand cmd = new SqlCommand("AddVehicle", connection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", vehicle.Name);
                cmd.Parameters.AddWithValue("@KM", vehicle.KM);
                cmd.Parameters.AddWithValue("@RegistrationNumber", vehicle.RegistrationNumber);
                cmd.Parameters.AddWithValue("@AgeGroup", vehicle.AgeGroup);
                cmd.Parameters.AddWithValue("@TowHook", vehicle.TowHook);
                cmd.Parameters.AddWithValue("@DriversLicenceClass", vehicle.DriversLicenceClass);
                cmd.Parameters.AddWithValue("@EngineSize", vehicle.EngineSize);
                cmd.Parameters.AddWithValue("@KmL", vehicle.KmL);
                cmd.Parameters.AddWithValue("@FuelType", vehicle.FuelType);
                cmd.Parameters.AddWithValue("@EnergyClass", vehicle.EnergyClass);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // Read - Get Vehicle by ID
        public Vehicle GetVehicle(int vehicleId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();


                SqlCommand cmd = new SqlCommand("GetVehicleById", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add the input parameter for the stored procedure
                cmd.Parameters.AddWithValue("@VehicleId", vehicleId);

                SqlDataReader reader = cmd.ExecuteReader();
                Vehicle vehicle = null;

                if (reader.Read())
                {
                    vehicle = new Vehicle(
                        Convert.ToInt32(reader["VehicleId"]),
                        reader["Name"].ToString(),
                        reader["Km"].ToString(),
                        reader["RegistrationNumber"].ToString(),
                        reader["AgeGroup"].ToString(),
                        Convert.ToBoolean(reader["TowHook"]),
                        reader["DriversLicenceClass"].ToString(),
                        reader["EngineSize"].ToString(),
                        Convert.ToDecimal(reader["KmL"]),
                        reader["FuelType"].ToString(),
                        reader["EnergyClass"].ToString()
                    );
                }

                return vehicle;
            }
        }

        public void GetVehicleTypeAndId(int vehicleId, out int VehicleSubId, out string VehicleType)
        {
            VehicleSubId = 0;
            VehicleType = "N/A";

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("GetVehicleByIdAndReturnTypeAndId", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add the input parameter for the stored procedure
                cmd.Parameters.AddWithValue("@VehicleId", vehicleId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Retrieve the vehicle type and sub ID if found
                        VehicleType = reader["VehicleType"].ToString();
                        VehicleSubId = reader["VehicleSubId"] is DBNull ? 0 : Convert.ToInt32(reader["VehicleSubId"]); // Set VehicleSubId, defaults to 0 if NULL
                    }
                }
            }
        }


        // Update - Update Vehicle
        public void UpdateVehicle(Vehicle vehicle)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            UPDATE Vehicles 
                            SET Name = @Name, KM = @Km, RegistrationNumber = @RegistrationNumber, AgeGroup = @AgeGroup, TowHook = @TowHook, 
                                DriversLicenceClass = @DriversLicenceClass, EngineSize = @EngineSize, KmL = @KmL, FuelType = @FuelType, EnergyClass = @EnergyClass
                            WHERE VehicleId = @VehicleId";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@Name", vehicle.Name);
                        cmd.Parameters.AddWithValue("@Km", vehicle.KM);
                        cmd.Parameters.AddWithValue("@RegistrationNumber", vehicle.RegistrationNumber);
                        cmd.Parameters.AddWithValue("@AgeGroup", vehicle.AgeGroup);
                        cmd.Parameters.AddWithValue("@TowHook", vehicle.TowHook);
                        cmd.Parameters.AddWithValue("@DriversLicenceClass", vehicle.DriversLicenceClass);
                        cmd.Parameters.AddWithValue("@EngineSize", vehicle.EngineSize);
                        cmd.Parameters.AddWithValue("@KmL", vehicle.KmL);
                        cmd.Parameters.AddWithValue("@FuelType", vehicle.FuelType);
                        cmd.Parameters.AddWithValue("@EnergyClass", vehicle.EnergyClass);
                        cmd.Parameters.AddWithValue("@VehicleId", vehicle.Id);

                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error updating vehicle in database: " + ex.Message);
                    }
                }
            }
        }

        // Delete - Delete Vehicle by ID
        public void DeleteVehicle(int vehicleId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = "DELETE FROM Vehicles WHERE VehicleId = @VehicleId";
                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@VehicleId", vehicleId);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error deleting vehicle from database: " + ex.Message);
                    }
                }
            }
        }
    }
}
