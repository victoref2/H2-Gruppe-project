using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        // Create - Add Truck
        public void AddTruck(Truck truck)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert into Vehicles table
                        string vehicleQuery = @"
                            INSERT INTO Vehicles (Name, KM, RegistrationNumber, AgeGroup, TowHook, DriversLicenceClass, EngineSize, KmL, FuelType, EnergyClass)
                            VALUES (@Name, @KM, @RegistrationNumber, @AgeGroup, @TowHook, @DriversLicenceClass, @EngineSize, @KmL, @FuelType, @EnergyClass);
                            SELECT SCOPE_IDENTITY();";
                        SqlCommand vehicleCmd = new SqlCommand(vehicleQuery, connection, transaction);
                        vehicleCmd.Parameters.AddWithValue("@Name", truck.Name);
                        vehicleCmd.Parameters.AddWithValue("@Km", truck.KM);
                        vehicleCmd.Parameters.AddWithValue("@RegistrationNumber", truck.RegistrationNumber);
                        vehicleCmd.Parameters.AddWithValue("@AgeGroup", truck.AgeGroup);
                        vehicleCmd.Parameters.AddWithValue("@TowHook", truck.TowHook);
                        vehicleCmd.Parameters.AddWithValue("@DriversLicenceClass", truck.DriversLicenceClass);
                        vehicleCmd.Parameters.AddWithValue("@EngineSize", decimal.Parse(truck.EngineSize.TrimEnd('L', 'l')));
                        vehicleCmd.Parameters.AddWithValue("@KmL", truck.KmL);
                        vehicleCmd.Parameters.AddWithValue("@FuelType", truck.FuelType);
                        vehicleCmd.Parameters.AddWithValue("@EnergyClass", truck.EnergyClass);

                        int vehicleId = Convert.ToInt32(vehicleCmd.ExecuteScalar());

                        // Insert into HeavyVehicles table
                        string heavyVehicleQuery = @"
                            INSERT INTO HeavyVehicles (VehicleId, MaxLoadCapacity, NumberOfAxles)
                            VALUES (@VehicleId, @MaxLoadCapacity, @NumberOfAxles);
                            SELECT SCOPE_IDENTITY();";
                        SqlCommand heavyVehicleCmd = new SqlCommand(heavyVehicleQuery, connection, transaction);
                        heavyVehicleCmd.Parameters.AddWithValue("@VehicleId", vehicleId);
                        heavyVehicleCmd.Parameters.AddWithValue("@MaxLoadCapacity", truck.MaxLoadCapacity);
                        heavyVehicleCmd.Parameters.AddWithValue("@NumberOfAxles", truck.NumberOfAxles);

                        int heavyVehicleId = Convert.ToInt32(heavyVehicleCmd.ExecuteScalar());

                        // Insert into Trucks table
                        string truckQuery = @"
                            INSERT INTO Trucks (HeavyVehicleId, Height, Weight, Length, LoadCapacity)
                            VALUES (@HeavyVehicleId, @Height, @Weight, @Length, @LoadCapacity);
                            SELECT SCOPE_IDENTITY();";
                        SqlCommand truckCmd = new SqlCommand(truckQuery, connection, transaction);
                        truckCmd.Parameters.AddWithValue("@HeavyVehicleId", heavyVehicleId);
                        truckCmd.Parameters.AddWithValue("@Height", truck.Height);
                        truckCmd.Parameters.AddWithValue("@Weight", truck.Weight);
                        truckCmd.Parameters.AddWithValue("@Length", truck.Length);
                        truckCmd.Parameters.AddWithValue("@LoadCapacity", truck.LoadCapacity);

                        int truckId = Convert.ToInt32(truckCmd.ExecuteScalar());

                        transaction.Commit();

                        truck.TruckId = truckId;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error adding truck to database: " + ex.Message);
                    }
                }
            }
        }

        // Read - Get Truck by ID
        public Truck GetTruckById(int truckId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT v.Name, v.KM, v.RegistrationNumber, v.AgeGroup, v.TowHook, v.DriversLicenceClass, v.EngineSize, v.KmL, v.FuelType, v.EnergyClass,
                           hv.MaxLoadCapacity, hv.NumberOfAxles,
                           t.Height, t.Weight, t.Length, t.LoadCapacity
                    FROM Trucks t
                    INNER JOIN HeavyVehicles hv ON t.HeavyVehicleId = hv.HeavyVehicleId
                    INNER JOIN Vehicles v ON hv.VehicleId = v.VehicleId
                    WHERE t.TruckId = @TruckId;";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@TruckId", truckId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Truck(
                            id: truckId.ToString(),
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
                            maxLoadCapacity: Convert.ToInt32(reader["MaxLoadCapacity"]),
                            numberOfAxles: Convert.ToInt32(reader["NumberOfAxles"]),
                            height: Convert.ToDecimal(reader["Height"]),
                            weight: Convert.ToDecimal(reader["Weight"]),
                            length: Convert.ToDecimal(reader["Length"]),
                            loadCapacity: Convert.ToDecimal(reader["LoadCapacity"])
                        )
                        {
                            TruckId = truckId
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        // Update - Update Truck
        public void UpdateTruck(Truck truck)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Update Vehicles table
                        string vehicleQuery = @"
                            UPDATE Vehicles
                            SET Name = @Name, KM = @KM, RegistrationNumber = @RegistrationNumber, AgeGroup = @AgeGroup, TowHook = @TowHook,
                                DriversLicenceClass = @DriversLicenceClass, EngineSize = @EngineSize, KmL = @KmL, FuelType = @FuelType, EnergyClass = @EnergyClass
                            WHERE VehicleId = (SELECT VehicleId FROM HeavyVehicles WHERE HeavyVehicleId = (SELECT HeavyVehicleId FROM Trucks WHERE TruckId = @TruckId))";
                        SqlCommand vehicleCmd = new SqlCommand(vehicleQuery, connection, transaction);
                        vehicleCmd.Parameters.AddWithValue("@Name", truck.Name);
                        vehicleCmd.Parameters.AddWithValue("@KM", truck.KM);
                        vehicleCmd.Parameters.AddWithValue("@RegistrationNumber", truck.RegistrationNumber);
                        vehicleCmd.Parameters.AddWithValue("@AgeGroup", truck.AgeGroup);
                        vehicleCmd.Parameters.AddWithValue("@TowHook", truck.TowHook);
                        vehicleCmd.Parameters.AddWithValue("@DriversLicenceClass", truck.DriversLicenceClass);
                        vehicleCmd.Parameters.AddWithValue("@EngineSize", decimal.Parse(truck.EngineSize.TrimEnd('L', 'l')));
                        vehicleCmd.Parameters.AddWithValue("@KmL", truck.KmL);
                        vehicleCmd.Parameters.AddWithValue("@FuelType", truck.FuelType);
                        vehicleCmd.Parameters.AddWithValue("@EnergyClass", truck.EnergyClass);
                        vehicleCmd.Parameters.AddWithValue("@TruckId", truck.TruckId);

                        vehicleCmd.ExecuteNonQuery();

                        // Update HeavyVehicles table
                        string heavyVehicleQuery = @"
                            UPDATE HeavyVehicles
                            SET MaxLoadCapacity = @MaxLoadCapacity, NumberOfAxles = @NumberOfAxles
                            WHERE HeavyVehicleId = (SELECT HeavyVehicleId FROM Trucks WHERE TruckId = @TruckId)";
                        SqlCommand heavyVehicleCmd = new SqlCommand(heavyVehicleQuery, connection, transaction);
                        heavyVehicleCmd.Parameters.AddWithValue("@MaxLoadCapacity", truck.MaxLoadCapacity);
                        heavyVehicleCmd.Parameters.AddWithValue("@NumberOfAxles", truck.NumberOfAxles);
                        heavyVehicleCmd.Parameters.AddWithValue("@TruckId", truck.TruckId);

                        heavyVehicleCmd.ExecuteNonQuery();

                        // Update Trucks table
                        string truckQuery = @"
                            UPDATE Trucks
                            SET Height = @Height, Weight = @Weight, Length = @Length, LoadCapacity = @LoadCapacity
                            WHERE TruckId = @TruckId";
                        SqlCommand truckCmd = new SqlCommand(truckQuery, connection, transaction);
                        truckCmd.Parameters.AddWithValue("@Height", truck.Height);
                        truckCmd.Parameters.AddWithValue("@Weight", truck.Weight);
                        truckCmd.Parameters.AddWithValue("@Length", truck.Length);
                        truckCmd.Parameters.AddWithValue("@LoadCapacity", truck.LoadCapacity);
                        truckCmd.Parameters.AddWithValue("@TruckId", truck.TruckId);

                        truckCmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error updating truck in database: " + ex.Message);
                    }
                }
            }
        }

        // Delete - Delete Truck
        public void DeleteTruck(int truckId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Retrieve VehicleId first
                        string getVehicleIdQuery = @"
                            SELECT hv.VehicleId
                            FROM Trucks t
                            INNER JOIN HeavyVehicles hv ON t.HeavyVehicleId = hv.HeavyVehicleId
                            WHERE t.TruckId = @TruckId;";
                        SqlCommand getVehicleIdCmd = new SqlCommand(getVehicleIdQuery, connection, transaction);
                        getVehicleIdCmd.Parameters.AddWithValue("@TruckId", truckId);
                        int vehicleId = (int)getVehicleIdCmd.ExecuteScalar();

                        // Delete from Trucks table
                        string deleteTruckQuery = "DELETE FROM Trucks WHERE TruckId = @TruckId;";
                        SqlCommand deleteTruckCmd = new SqlCommand(deleteTruckQuery, connection, transaction);
                        deleteTruckCmd.Parameters.AddWithValue("@TruckId", truckId);
                        deleteTruckCmd.ExecuteNonQuery();

                        // Delete from HeavyVehicles table
                        string deleteHeavyVehicleQuery = "DELETE FROM HeavyVehicles WHERE VehicleId = @VehicleId;";
                        SqlCommand deleteHeavyVehicleCmd = new SqlCommand(deleteHeavyVehicleQuery, connection, transaction);
                        deleteHeavyVehicleCmd.Parameters.AddWithValue("@VehicleId", vehicleId);
                        deleteHeavyVehicleCmd.ExecuteNonQuery();

                        // Delete from Vehicles table
                        string deleteVehicleQuery = "DELETE FROM Vehicles WHERE VehicleId = @VehicleId;";
                        SqlCommand deleteVehicleCmd = new SqlCommand(deleteVehicleQuery, connection, transaction);
                        deleteVehicleCmd.Parameters.AddWithValue("@VehicleId", vehicleId);
                        deleteVehicleCmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error deleting truck from database: " + ex.Message);
                    }
                }
            }
        }
    }
}
