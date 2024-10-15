using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public int AddPVehicleFlow(PrivateVehicle privateVehicle, Vehicle vehicle, NormalVehicle normalVehicle)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int vehicleId = AddVehicle(connection, transaction, vehicle);

                    int normalVehicleId = AddNormalVehicle(connection, transaction, vehicleId, normalVehicle);

                    AddPrivateVehicle(connection, transaction, normalVehicleId, privateVehicle);

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

        private void AddPrivateVehicle(SqlConnection connection, SqlTransaction transaction, int normalVehicleId, PrivateVehicle privateVehicle)
        {
            using (SqlCommand cmd = new SqlCommand("AddPrivateVehicles", connection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NormalVehiclesId", normalVehicleId);
                cmd.Parameters.AddWithValue("@IsofixMount", privateVehicle.IsofixMount);

                cmd.ExecuteNonQuery();
            }
        }

        public PrivateVehicle GetPrivatVHById(int vehicleId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("GetAPrivateVehicle", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@PrivateVehicleId", vehicleId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new PrivateVehicle(
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
                            isofixMount: Convert.ToBoolean(reader["IsofixMount"])  // New field from the updated SP
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public List<PrivateVehicle> GetAllPrivateVH()
        {
            List<PrivateVehicle> privateVehicles = new List<PrivateVehicle>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("GetAllPrivateVehicles", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PrivateVehicle vehicle = new PrivateVehicle(
                            id: Convert.ToInt32(reader["PrivateVehicleId"]),
                            name: reader["VehicleName"].ToString(),
                            km: reader["KM"].ToString(),
                            registrationNumber: reader["RegistrationNumber"].ToString(),
                            ageGroup: reader["AgeGroup"].ToString(),
                            towHook: Convert.ToBoolean(reader["TowHook"]),  // Add this field
                            driversLicenceClass: reader["DriversLicenceClass"].ToString(),  // Add this field
                            engineSize: reader["EngineSize"].ToString() + "L",
                            kmL: Convert.ToDecimal(reader["KmL"]),
                            fuelType: reader["FuelType"].ToString(),
                            energyClass: reader["EnergyClass"].ToString(),
                            numberOfSeats: Convert.ToInt32(reader["NumberOfSeats"]),
                            trunkDimensions: reader["TrunkDimensions"].ToString(),
                            isCommercial: Convert.ToBoolean(reader["IsCommercial"]),
                            isofixMount: Convert.ToBoolean(reader["IsofixMount"])
                        );


                        privateVehicles.Add(vehicle);
                    }
                }
            }

            return privateVehicles;
        }

        public void EditPrivateVehicle(PrivateVehicle privateVehicle)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("EditPrivateVehicle", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                cmd.Parameters.AddWithValue("@PrivateVehicleId", privateVehicle.Id);
                cmd.Parameters.AddWithValue("@IsofixMount", privateVehicle.IsofixMount);
                cmd.Parameters.AddWithValue("@NumberOfSeats", privateVehicle.NumberOfSeats);
                cmd.Parameters.AddWithValue("@TrunkDimensions", privateVehicle.TrunkDimensions);
                cmd.Parameters.AddWithValue("@VehicleName", privateVehicle.Name);
                cmd.Parameters.AddWithValue("@KM", privateVehicle.KM);
                cmd.Parameters.AddWithValue("@RegistrationNumber", privateVehicle.RegistrationNumber);
                cmd.Parameters.AddWithValue("@AgeGroup", privateVehicle.AgeGroup);
                cmd.Parameters.AddWithValue("@FuelType", privateVehicle.FuelType);
                cmd.Parameters.AddWithValue("@EnergyClass", privateVehicle.EnergyClass);

                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void DeletePrivateVH(int VHid)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DeleteAPrivateVehicle", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("@PrivateVehicleId", VHid);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
