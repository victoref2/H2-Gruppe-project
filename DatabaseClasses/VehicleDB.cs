using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public void AddVehicle(Vehicle vehicle)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            INSERT INTO Vehicles (Name, KM, RegistrationNumber, AgeGroup, TowHook, DriversLicenceClass, EngineSize, KmL, FuelType, EnergyClass) 
                            VALUES (@Name, @Km, @RegistrationNumber, @AgeGroup, @TowHook, @DriversLicenceClass, @EngineSize, @KmL, @FuelType, @EnergyClass);
                            SELECT SCOPE_IDENTITY();";
                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@Name", vehicle.Name);
                        cmd.Parameters.AddWithValue("@Km", vehicle.KM);
                        cmd.Parameters.AddWithValue("@RegistrationNumber", vehicle.RegristrationNumber);
                        cmd.Parameters.AddWithValue("@AgeGroup", vehicle.AgeGroup);
                        cmd.Parameters.AddWithValue("@TowHook", vehicle.TowHook);
                        cmd.Parameters.AddWithValue("@DriversLicenceClass", vehicle.DriversLicenceClass);
                        cmd.Parameters.AddWithValue("@EngineSize", vehicle.EngineSize);
                        cmd.Parameters.AddWithValue("@KmL", vehicle.KmL);
                        cmd.Parameters.AddWithValue("@FuelType", vehicle.FuelType);
                        cmd.Parameters.AddWithValue("@EnergyClass", vehicle.EnergyClass);

                        int vehicleId = Convert.ToInt32(cmd.ExecuteScalar());
                        vehicle.Id = vehicleId.ToString(); 


                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error adding vehicle to database: " + ex.Message);
                    }
                }
            }
        }

        public Vehicle GetVehicle(int vehicleId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"SELECT * FROM Vehicles WHERE VehicleId = @VehicleId";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@VehicleId", vehicleId);

                SqlDataReader reader = cmd.ExecuteReader();
                Vehicle vehicle = null;

                if (reader.Read())
                {
                    vehicle = new Vehicle(
                        reader["VehicleId"].ToString(),
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
