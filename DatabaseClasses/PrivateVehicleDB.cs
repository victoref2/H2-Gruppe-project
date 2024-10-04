using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public void AddPrivateVehicle(PrivateVehicle privateVehicle)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            EXEC AddPrivateVehicle 
                                @Name, @KM, @RegistrationNumber, @AgeGroup, @TowHook, @DriversLicenceClass, 
                                @EngineSize, @KmL, @FuelType, @EnergyClass, 
                                @NumberOfSeats, @TrunkDimensions, @IsCommercial, @IsofixMount;
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);

                        cmd.Parameters.AddWithValue("@Name", privateVehicle.Name);
                        cmd.Parameters.AddWithValue("@KM", privateVehicle.KM);
                        cmd.Parameters.AddWithValue("@RegistrationNumber", privateVehicle.RegistrationNumber);
                        cmd.Parameters.AddWithValue("@AgeGroup", privateVehicle.AgeGroup);
                        cmd.Parameters.AddWithValue("@TowHook", privateVehicle.TowHook);
                        cmd.Parameters.AddWithValue("@DriversLicenceClass", privateVehicle.DriversLicenceClass);
                        cmd.Parameters.AddWithValue("@EngineSize", decimal.Parse(privateVehicle.EngineSize.TrimEnd('L', 'l')));
                        cmd.Parameters.AddWithValue("@KmL", privateVehicle.KmL);
                        cmd.Parameters.AddWithValue("@FuelType", privateVehicle.FuelType);
                        cmd.Parameters.AddWithValue("@EnergyClass", privateVehicle.EnergyClass);
                        cmd.Parameters.AddWithValue("@NumberOfSeats", privateVehicle.NumberOfSeats);
                        cmd.Parameters.AddWithValue("@TrunkDimensions", privateVehicle.TrunkDimensions);
                        cmd.Parameters.AddWithValue("@IsCommercial", privateVehicle.IsCommercial);
                        cmd.Parameters.AddWithValue("@IsofixMount", privateVehicle.IsofixMount);

                        int vehicleId = Convert.ToInt32(cmd.ExecuteScalar());
                        privateVehicle.Id = vehicleId.ToString();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error adding private vehicle to the database: " + ex.Message);
                    }
                }
            }
        }

        public PrivateVehicle GetPrivateVehicleById(int vehicleId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT v.*, nv.NormalVehicleId, nv.NumberOfSeats, nv.TrunkDimensions, nv.IsCommercial, pv.IsofixMount
                    FROM Vehicles v
                    INNER JOIN NormalVehicles nv ON v.VehicleId = nv.VehicleId
                    INNER JOIN PrivateVehicles pv ON nv.NormalVehicleId = pv.NormalVehicleId
                    WHERE v.VehicleId = @VehicleId;";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@VehicleId", vehicleId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new PrivateVehicle(
                            id: reader["VehicleId"].ToString(),
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
                            numberOfSeats: Convert.ToInt32(reader["NumberOfSeats"]),
                            trunkDimensions: reader["TrunkDimensions"].ToString(),
                            isCommercial: Convert.ToBoolean(reader["IsCommercial"]),
                            isofixMount: Convert.ToBoolean(reader["IsofixMount"])
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
