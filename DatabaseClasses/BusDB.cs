using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public void AddBus(Bus bus)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
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


                        string heavyVehicleQuery = @"
                            INSERT INTO HeavyVehicles (VehicleId, MaxLoadCapacity, NumberOfAxles)
                            VALUES (@VehicleId, @MaxLoadCapacity, @NumberOfAxles);
                            SELECT SCOPE_IDENTITY();";
                        SqlCommand heavyVehicleCmd = new SqlCommand(heavyVehicleQuery, connection, transaction);
                        heavyVehicleCmd.Parameters.AddWithValue("@VehicleId", vehicleId);
                        heavyVehicleCmd.Parameters.AddWithValue("@MaxLoadCapacity", bus.MaxLoadCapacity);
                        heavyVehicleCmd.Parameters.AddWithValue("@NumberOfAxles", bus.NumberOfAxles);

                        int heavyVehicleId = Convert.ToInt32(heavyVehicleCmd.ExecuteScalar());

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

                
        public void DeleteBus(int busId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string deleteBusQuery = "DELETE FROM Buses WHERE BusId = @BusId;";
                        SqlCommand deleteBusCmd = new SqlCommand(deleteBusQuery, connection, transaction);
                        deleteBusCmd.Parameters.AddWithValue("@BusId", busId);
                        deleteBusCmd.ExecuteNonQuery();

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
