using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using H2_Gruppe_project.Classes;
using Microsoft.VisualBasic.FileIO;
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

        public List<Truck> GetAllTrucks()
        {
            List<Truck> trucks = new List<Truck>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("GetAllTrucks", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Truck truck = new Truck(
                            id: Convert.ToInt32(reader["TruckId"]),
                            name: reader["VehicleName"].ToString(),
                            km: reader["KM"].ToString(),
                            registrationNumber: reader["RegistrationNumber"].ToString(),
                            ageGroup: reader["AgeGroup"].ToString(),
                            towHook: Convert.ToBoolean(reader["TowHook"]),
                            driversLicenceClass: reader["DriversLicenceClass"].ToString(),
                            engineSize: reader["EngineSize"].ToString(), // Assuming engine size is a string
                            kmL: Convert.ToDecimal(reader["KmL"]),
                            fuelType: reader["FuelType"].ToString(),
                            energyClass: reader["EnergyClass"].ToString(),
                            maxLoadCapacity: Convert.ToInt32(reader["MaxLoadCapacity"]),
                            numberOfAxles: Convert.ToInt32(reader["NumberOfAxles"]),
                            height: Convert.ToDecimal(reader["Height"]),
                            weight: Convert.ToDecimal(reader["Weight"]),
                            length: Convert.ToDecimal(reader["Length"]),
                            loadCapacity: Convert.ToDecimal(reader["LoadCapacity"])
                        );

                        trucks.Add(truck);
                    }
                }
            }

            return trucks;
        }

        public Truck GetTruckById(int truckId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("GetATruck", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@TruckId", truckId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Truck(
                            id: Convert.ToInt32(reader["TruckId"]),
                            name: reader["VehicleName"].ToString(),
                            km: reader["KM"].ToString(),
                            registrationNumber: reader["RegistrationNumber"].ToString(),
                            ageGroup: reader["AgeGroup"].ToString(),
                            towHook: Convert.ToBoolean(reader["TowHook"]),
                            driversLicenceClass: reader["DriversLicenceClass"].ToString(),
                            engineSize: reader["EngineSize"].ToString() + "L", // Assuming engine size needs "L"
                            kmL: Convert.ToDecimal(reader["KmL"]),
                            fuelType: reader["FuelType"].ToString(),
                            energyClass: reader["EnergyClass"].ToString(),
                            maxLoadCapacity: Convert.ToInt32(reader["MaxLoadCapacity"]),
                            numberOfAxles: Convert.ToInt32(reader["NumberOfAxles"]),
                            height: Convert.ToDecimal(reader["Height"]),
                            weight: Convert.ToDecimal(reader["Weight"]),
                            length: Convert.ToDecimal(reader["Length"]),
                            loadCapacity: Convert.ToDecimal(reader["LoadCapacity"])
                        );
                    }
                    else
                    {
                        return null; // Or handle the case where no truck is found
                    }
                }
            }
        }

        public void EditTruck(Truck truck)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("EditTruck", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                cmd.Parameters.AddWithValue("@TruckId", truck.TruckId);
                cmd.Parameters.AddWithValue("@Height", truck.Height);
                cmd.Parameters.AddWithValue("@Weight", truck.Weight);
                cmd.Parameters.AddWithValue("@Length", truck.Length);
                cmd.Parameters.AddWithValue("@LoadCapacity", truck.LoadCapacity);
                cmd.Parameters.AddWithValue("@MaxLoadCapacity", truck.MaxLoadCapacity);
                cmd.Parameters.AddWithValue("@NumberOfAxles", truck.NumberOfAxles);
                cmd.Parameters.AddWithValue("@VehicleName", truck.Name);
                cmd.Parameters.AddWithValue("@KM", truck.KM);
                cmd.Parameters.AddWithValue("@RegistrationNumber", truck.RegistrationNumber);
                cmd.Parameters.AddWithValue("@AgeGroup", truck.AgeGroup);
                cmd.Parameters.AddWithValue("@FuelType", truck.FuelType);
                cmd.Parameters.AddWithValue("@EnergyClass", truck.EnergyClass);

                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }


        // Delete - Delete Truck
        public void DeleteTruck(int truckId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("DeleteTruck", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                cmd.Parameters.AddWithValue("@TruckId", truckId);

                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

    }
}
