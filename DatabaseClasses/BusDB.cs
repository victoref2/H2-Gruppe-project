using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using H2_Gruppe_project.Classes;
using Tmds.DBus.Protocol;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public int AddBusFlow(Bus bus, Vehicle vehicle, HeavyVehicle heavyVehicle)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int vehicleId = AddVehicle(connection, transaction, vehicle);

                    int HeavyVHId = AddHeavyVehicle(connection, transaction, vehicleId, heavyVehicle);

                    AddBus(connection, transaction, HeavyVHId, bus);

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
        // Create - Add Bus
        public void AddBus(SqlConnection connection, SqlTransaction transaction, int HeavyVHId, Bus bus)
        {
            using (SqlCommand cmd = new SqlCommand("AddBus", connection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HeavyVehicleId", HeavyVHId);
                cmd.Parameters.AddWithValue("@Height", bus.Height);
                cmd.Parameters.AddWithValue("@Weight", bus.Weight);
                cmd.Parameters.AddWithValue("@Length", bus.Length);
                cmd.Parameters.AddWithValue("@NumberOfSeats", bus.NumberOfSeats);
                cmd.Parameters.AddWithValue("@NumberOfSleepingPlaces", bus.NumberOfSleepingPlaces);
                cmd.Parameters.AddWithValue("@HasToilet", bus.HasToilet);

                cmd.ExecuteNonQuery();
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
                            id: Convert.ToInt32(busId),
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
                        vehicleCmd.Parameters.AddWithValue("@RegistrationNumber", bus.RegistrationNumber);
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
