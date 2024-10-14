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
        public int AddTruckFlow(Truck truck, Vehicle vehicle, HeavyVehicle heavyVehicle)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int vehicleId = AddVehicle(connection, transaction, vehicle);

                    int HeavyVHId = AddHeavyVehicle(connection, transaction, vehicleId, heavyVehicle);

                    AddTruck(connection, transaction, HeavyVHId, truck);

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

        // Create - Add Truck
        public void AddTruck(SqlConnection connection, SqlTransaction transaction,int HeavyVHId,Truck truck)
        {
            using (SqlCommand cmd = new SqlCommand("AddTruck", connection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HeavyVehicleId", HeavyVHId);
                cmd.Parameters.AddWithValue("@Height", truck.Height);
                cmd.Parameters.AddWithValue("@Weight", truck.Weight);
                cmd.Parameters.AddWithValue("@Length", truck.Length);
                cmd.Parameters.AddWithValue("@LoadCapacity", truck.LoadCapacity);

                cmd.ExecuteNonQuery();
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
                            id: truckId,
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
