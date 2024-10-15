using System;
using System.Collections.Generic;
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

                    int heavyVHId = AddHeavyVehicle(connection, transaction, vehicleId, heavyVehicle);

                    AddBus(connection, transaction, heavyVHId, bus);

                    transaction.Commit();
                    return vehicleId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error during vehicle creation", ex);
                }
            }
        }

        public void AddBus(SqlConnection connection, SqlTransaction transaction, int heavyVHId, Bus bus)
        {
            using (SqlCommand cmd = new SqlCommand("AddBus", connection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HeavyVehicleId", heavyVHId);
                cmd.Parameters.AddWithValue("@Height", bus.Height);
                cmd.Parameters.AddWithValue("@Weight", bus.Weight);
                cmd.Parameters.AddWithValue("@Length", bus.Length);
                cmd.Parameters.AddWithValue("@NumberOfSeats", bus.NumberOfSeats);
                cmd.Parameters.AddWithValue("@NumberOfSleepingPlaces", bus.NumberOfSleepingPlaces);
                cmd.Parameters.AddWithValue("@HasToilet", bus.HasToilet);

                cmd.ExecuteNonQuery();
            }
        }

        public List<Bus> GetAllBuses()
        {
            List<Bus> buses = new List<Bus>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("GetAllBuses", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Bus bus = new Bus(
                            id: Convert.ToInt32(reader["BusId"]),
                            name: reader["VehicleName"].ToString(),
                            km: reader["KM"].ToString(),
                            registrationNumber: reader["RegistrationNumber"].ToString(),
                            ageGroup: reader["AgeGroup"].ToString(),
                            towHook: Convert.ToBoolean(reader["TowHook"]),
                            driversLicenceClass: reader["DriversLicenceClass"].ToString(),
                            engineSize: reader["EngineSize"].ToString(),
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

                        buses.Add(bus);
                    }
                }
            }

            return buses;
        }

        public Bus GetABus(int busId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("GetABus", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@BusId", busId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Bus(
                            id: Convert.ToInt32(reader["BusId"]),
                            name: reader["VehicleName"].ToString(),
                            km: reader["KM"].ToString(),
                            registrationNumber: reader["RegistrationNumber"].ToString(),
                            ageGroup: reader["AgeGroup"].ToString(),
                            towHook: Convert.ToBoolean(reader["TowHook"]),
                            driversLicenceClass: reader["DriversLicenceClass"].ToString(),
                            engineSize: reader["EngineSize"].ToString(),
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
                }
            }

            return null; // Return null if the bus is not found
        }

        public void EditBus(Bus bus)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("EditBus", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BusId", bus.Id); // Assuming you have an Id property in Bus
                    cmd.Parameters.AddWithValue("@Height", bus.Height);
                    cmd.Parameters.AddWithValue("@Weight", bus.Weight);
                    cmd.Parameters.AddWithValue("@Length", bus.Length);
                    cmd.Parameters.AddWithValue("@NumberOfSeats", bus.NumberOfSeats);
                    cmd.Parameters.AddWithValue("@NumberOfSleepingPlaces", bus.NumberOfSleepingPlaces);
                    cmd.Parameters.AddWithValue("@HasToilet", bus.HasToilet);
                    cmd.Parameters.AddWithValue("@MaxLoadCapacity", bus.MaxLoadCapacity);
                    cmd.Parameters.AddWithValue("@NumberOfAxles", bus.NumberOfAxles);
                    cmd.Parameters.AddWithValue("@VehicleName", bus.Name);
                    cmd.Parameters.AddWithValue("@KM", bus.KM);
                    cmd.Parameters.AddWithValue("@RegistrationNumber", bus.RegistrationNumber);
                    cmd.Parameters.AddWithValue("@AgeGroup", bus.AgeGroup);
                    cmd.Parameters.AddWithValue("@FuelType", bus.FuelType);
                    cmd.Parameters.AddWithValue("@EnergyClass", bus.EnergyClass);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteABus(int busId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("DeleteABus", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BusId", busId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
