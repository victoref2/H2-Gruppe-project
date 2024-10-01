using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        // Create - Add Bus
        public void AddBus(Bus bus)
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
                        vehicleCmd.Parameters.AddWithValue("@Name", bus.Name);
                        vehicleCmd.Parameters.AddWithValue("@KM", bus.KM);
                        vehicleCmd.Parameters.AddWithValue("@RegistrationNumber", bus.RegristrationNumber);
                        vehicleCmd.Parameters.AddWithValue("@AgeGroup", bus.AgeGroup);
                        vehicleCmd.Parameters.AddWithValue("@TowHook", bus.TowHook);
                        vehicleCmd.Parameters.AddWithValue("@DriversLicenceClass", bus.DriversLicenceClass);
                        vehicleCmd.Parameters.AddWithValue("@EngineSize", decimal.Parse(bus.EngineSize.TrimEnd('L', 'l')));
                        vehicleCmd.Parameters.AddWithValue("@KmL", bus.KmL);
                        vehicleCmd.Parameters.AddWithValue("@FuelType", bus.FuelType);
                        vehicleCmd.Parameters.AddWithValue("@EnergyClass", bus.EnergyClass);

                        int vehicleId = Convert.ToInt32(vehicleCmd.ExecuteScalar());

                        // Insert into HeavyVehicles table
                        string heavyVehicleQuery = @"
                            INSERT INTO HeavyVehicles (VehicleId, MaxLoadCapacity, NumberOfAxles)
                            VALUES (@VehicleId, @MaxLoadCapacity, @NumberOfAxles);
                            SELECT SCOPE_IDENTITY();";
                        SqlCommand heavyVehicleCmd = new SqlCommand(heavyVehicleQuery, connection, transaction);
                        heavyVehicleCmd.Parameters.AddWithValue("@VehicleId", vehicleId);
                        heavyVehicleCmd.Parameters.AddWithValue("@MaxLoadCapacity", bus.MaxLoadCapacity);
                        heavyVehicleCmd.Parameters.AddWithValue("@NumberOfAxles", bus.NumberOfAxles);

                        int heavyVehicleId = Convert.ToInt32(heavyVehicleCmd.ExecuteScalar());

                        // Insert into Buses table
                        string busQuery = @"
                            INSERT INTO Buses (HeavyVehicleId, Height, Weight, Length, NumberOfSeats, NumberOfSleepingPlaces, HasToilet)
                            VALUES (@HeavyVehicleId, @Height, @Weight, @Length, @NumberOfSeats, @NumberOfSleepingPlaces, @HasToilet);
                            SELECT SCOPE_IDENTITY();";
                        SqlCommand busCmd = new SqlCommand(busQuery, connection, transaction);
                        busCmd.Parameters.AddWithValue("@HeavyVehicleId", heavyVehicleId);
                        busCmd.Parameters.AddWithValue("@Height", bus.Height);
                        busCmd.Parameters.AddWithValue("@Weight", bus.Weight);
                        busCmd.Parameters.AddWithValue("@Length", bus.Length);
                        busCmd.Parameters.AddWithValue("@NumberOfSeats", bus.NumberOfSeats);
                        busCmd.Parameters.AddWithValue("@NumberOfSleepingPlaces", bus.NumberOfSleepingPlaces);
                        busCmd.Parameters.AddWithValue("@HasToilet", bus.HasToilet);

                        int busId = Convert.ToInt32(busCmd.ExecuteScalar());

                        transaction.Commit();
                        bus.BusId = busId;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error adding bus to database: " + ex.Message);
                    }
                }
            }
        }

        // Read - Get Bus by ID
        public Bus GetBusById(int busId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT v.Name, v.KM, v.RegistrationNumber, v.AgeGroup, v.TowHook, v.DriversLicenceClass, v.EngineSize, v.KmL, v.FuelType, v.EnergyClass,
                           hv.MaxLoadCapacity, hv.NumberOfAxles,
                           b.Height, b.Weight, b.Length, b.NumberOfSeats, b.NumberOfSleepingPlaces, b.HasToilet
                    FROM Buses b
                    INNER JOIN HeavyVehicles hv ON b.HeavyVehicleId = hv.HeavyVehicleId
                    INNER JOIN Vehicles v ON hv.VehicleId = v.VehicleId
                    WHERE b.BusId = @BusId;";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@BusId", busId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Bus(
                            id: busId.ToString(),
                            name: reader["Name"].ToString(),
                            km: reader["KM"].ToString(),
                            regristrationNumber: reader["RegistrationNumber"].ToString(),
                            ageGroup: reader["AgeGroup"].ToString(),
                            towHook: Convert.ToBoolean(reader["TowHook"]),
                            engineSize: reader["EngineSize"].ToString() + "L",
                            kmL: Convert.ToDecimal(reader["KmL"]),
                            fuelType: reader["FuelType"].ToString(),
                            energyClass: reader["EnergyClass"].ToString(),
                            maxLoadCapacity: Convert.ToInt32(reader["MaxLoadCapacity"]),
                            numberOfAxles: Convert.ToInt32(reader["NumberOfAxles"]),
                            height: Convert.ToDecimal(reader["Height"]),
                            weight: Convert.ToDecimal(reader["Weight"]),
                            length: Convert.ToDecimal(reader["Length"]),
                            numberOfSeats: Convert.ToInt32(reader["NumberOfSeats"]),
                            numberOfSleepingPlaces: Convert.ToInt32(reader["NumberOfSleepingPlaces"]),
                            hasToilet: Convert.ToBoolean(reader["HasToilet"])
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        // Update - Update Bus
        public void UpdateBus(Bus bus)
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
                            SET Name = @Name, KM = @KM, RegistrationNumber = @RegistrationNumber, AgeGroup = @AgeGroup, 
                                TowHook = @TowHook, DriversLicenceClass = @DriversLicenceClass, EngineSize = @EngineSize, 
                                KmL = @KmL, FuelType = @FuelType, EnergyClass = @EnergyClass
                            WHERE VehicleId = (SELECT hv.VehicleId FROM HeavyVehicles hv INNER JOIN Buses b ON hv.HeavyVehicleId = b.HeavyVehicleId WHERE b.BusId = @BusId)";
                        SqlCommand vehicleCmd = new SqlCommand(vehicleQuery, connection, transaction);
                        vehicleCmd.Parameters.AddWithValue("@Name", bus.Name);
                        vehicleCmd.Parameters.AddWithValue("@KM", bus.KM);
                        vehicleCmd.Parameters.AddWithValue("@RegistrationNumber", bus.RegristrationNumber);
                        vehicleCmd.Parameters.AddWithValue("@AgeGroup", bus.AgeGroup);
                        vehicleCmd.Parameters.AddWithValue("@TowHook", bus.TowHook);
                        vehicleCmd.Parameters.AddWithValue("@DriversLicenceClass", bus.DriversLicenceClass);
                        vehicleCmd.Parameters.AddWithValue("@EngineSize", decimal.Parse(bus.EngineSize.TrimEnd('L', 'l')));
                        vehicleCmd.Parameters.AddWithValue("@KmL", bus.KmL);
                        vehicleCmd.Parameters.AddWithValue("@FuelType", bus.FuelType);
                        vehicleCmd.Parameters.AddWithValue("@EnergyClass", bus.EnergyClass);
                        vehicleCmd.Parameters.AddWithValue("@BusId", bus.BusId);

                        vehicleCmd.ExecuteNonQuery();

                        // Update HeavyVehicles table
                        string heavyVehicleQuery = @"
                            UPDATE HeavyVehicles
                            SET MaxLoadCapacity = @MaxLoadCapacity, NumberOfAxles = @NumberOfAxles
                            WHERE HeavyVehicleId = (SELECT HeavyVehicleId FROM Buses WHERE BusId = @BusId)";
                        SqlCommand heavyVehicleCmd = new SqlCommand(heavyVehicleQuery, connection, transaction);
                        heavyVehicleCmd.Parameters.AddWithValue("@MaxLoadCapacity", bus.MaxLoadCapacity);
                        heavyVehicleCmd.Parameters.AddWithValue("@NumberOfAxles", bus.NumberOfAxles);
                        heavyVehicleCmd.Parameters.AddWithValue("@BusId", bus.BusId);

                        heavyVehicleCmd.ExecuteNonQuery();

                        // Update Buses table
                        string busQuery = @"
                            UPDATE Buses
                            SET Height = @Height, Weight = @Weight, Length = @Length, NumberOfSeats = @NumberOfSeats,
                                NumberOfSleepingPlaces = @NumberOfSleepingPlaces, HasToilet = @HasToilet
                            WHERE BusId = @BusId";
                        SqlCommand busCmd = new SqlCommand(busQuery, connection, transaction);
                        busCmd.Parameters.AddWithValue("@Height", bus.Height);
                        busCmd.Parameters.AddWithValue("@Weight", bus.Weight);
                        busCmd.Parameters.AddWithValue("@Length", bus.Length);
                        busCmd.Parameters.AddWithValue("@NumberOfSeats", bus.NumberOfSeats);
                        busCmd.Parameters.AddWithValue("@NumberOfSleepingPlaces", bus.NumberOfSleepingPlaces);
                        busCmd.Parameters.AddWithValue("@HasToilet", bus.HasToilet);
                        busCmd.Parameters.AddWithValue("@BusId", bus.BusId);

                        busCmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error updating bus in database: " + ex.Message);
                    }
                }
            }
        }

        // Delete - Delete Bus
        public void DeleteBus(int busId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Retrieve the VehicleId first
                        string getVehicleIdQuery = @"
                            SELECT hv.VehicleId
                            FROM Buses b
                            INNER JOIN HeavyVehicles hv ON b.HeavyVehicleId = hv.HeavyVehicleId
                            WHERE b.BusId = @BusId;";
                        SqlCommand getVehicleIdCmd = new SqlCommand(getVehicleIdQuery, connection, transaction);
                        getVehicleIdCmd.Parameters.AddWithValue("@BusId", busId);
                        int vehicleId = (int)getVehicleIdCmd.ExecuteScalar();

                        // Delete from Buses table
                        string deleteBusQuery = "DELETE FROM Buses WHERE BusId = @BusId;";
                        SqlCommand deleteBusCmd = new SqlCommand(deleteBusQuery, connection, transaction);
                        deleteBusCmd.Parameters.AddWithValue("@BusId", busId);
                        deleteBusCmd.ExecuteNonQuery();

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
                        throw new Exception("Error deleting bus from database: " + ex.Message);
                    }
                }
            }
        }
    }
}
