using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
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

                SqlCommand cmd = new SqlCommand("GetACommercialVehicle", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CommercialVehicleId", vehicleId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ComercialVehicle(
                            id: Convert.ToInt32(reader["VehicleId"]),
                            name: reader["VehicleName"].ToString(),
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

        public List<ComercialVehicle> GetAllCommercialVH()
        {
            List<ComercialVehicle> commercialVehicles = new List<ComercialVehicle>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("GetAllCommercialVehicles", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ComercialVehicle vehicle = new ComercialVehicle(
                            id: Convert.ToInt32(reader["VehicleId"]),
                            name: reader["VehicleName"].ToString(),
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

                        commercialVehicles.Add(vehicle);
                    }
                }
            }

            return commercialVehicles;
        }

        public void EditCommercialVH(ComercialVehicle comercialVehicle)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand("EditCommercialVehicle", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                // Add parameters for the stored procedure
                command.Parameters.AddWithValue("@VehicleId", comercialVehicle.Id);
                command.Parameters.AddWithValue("@VehicleName", comercialVehicle.Name);
                command.Parameters.AddWithValue("@KM", comercialVehicle.KM);
                command.Parameters.AddWithValue("@RegistrationNumber", comercialVehicle.RegistrationNumber);
                command.Parameters.AddWithValue("@AgeGroup", comercialVehicle.AgeGroup);
                command.Parameters.AddWithValue("@TowHook", comercialVehicle.TowHook);
                command.Parameters.AddWithValue("@DriversLicenceClass", comercialVehicle.DriversLicenceClass);
                command.Parameters.AddWithValue("@EngineSize", comercialVehicle.EngineSize);
                command.Parameters.AddWithValue("@KmL", comercialVehicle.KmL);
                command.Parameters.AddWithValue("@FuelType", comercialVehicle.FuelType);
                command.Parameters.AddWithValue("@EnergyClass", comercialVehicle.EnergyClass);
                command.Parameters.AddWithValue("@NumberOfSeats", comercialVehicle.NumberOfSeats);
                command.Parameters.AddWithValue("@TrunkDimensions", comercialVehicle.TrunkDimensions);
                command.Parameters.AddWithValue("@IsCommercial", comercialVehicle.IsCommercial);
                command.Parameters.AddWithValue("@RollCage", comercialVehicle.RollCage);
                command.Parameters.AddWithValue("@LoadCapacity", comercialVehicle.LoadCapacity);

                // Execute the command
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void DeleteCommercialVH(int commercialVehicleId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DeleteACommercialVehicle", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("@VehicleId", commercialVehicleId);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

    }
}
