using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public void AddComercialVehicle(ComercialVehicle comercialVehicle)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            EXEC AddComercialVehicle 
                                @Name, @KM, @RegistrationNumber, @AgeGroup, @TowHook, @DriversLicenceClass, 
                                @EngineSize, @KmL, @FuelType, @EnergyClass, 
                                @NumberOfSeats, @TrunkDimensions, @IsCommercial, @RollCage, @LoadCapacity;
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);

                        cmd.Parameters.AddWithValue("@Name", comercialVehicle.Name);
                        cmd.Parameters.AddWithValue("@KM", comercialVehicle.KM);
                        cmd.Parameters.AddWithValue("@RegistrationNumber", comercialVehicle.RegristrationNumber);
                        cmd.Parameters.AddWithValue("@AgeGroup", comercialVehicle.AgeGroup);
                        cmd.Parameters.AddWithValue("@TowHook", comercialVehicle.TowHook);
                        cmd.Parameters.AddWithValue("@DriversLicenceClass", comercialVehicle.DriversLicenceClass);
                        cmd.Parameters.AddWithValue("@EngineSize", decimal.Parse(comercialVehicle.EngineSize.TrimEnd('L', 'l')));
                        cmd.Parameters.AddWithValue("@KmL", comercialVehicle.KmL);
                        cmd.Parameters.AddWithValue("@FuelType", comercialVehicle.FuelType);
                        cmd.Parameters.AddWithValue("@EnergyClass", comercialVehicle.EnergyClass);
                        cmd.Parameters.AddWithValue("@NumberOfSeats", comercialVehicle.NumberOfSeats);
                        cmd.Parameters.AddWithValue("@TrunkDimensions", comercialVehicle.TrunkDimensions);
                        cmd.Parameters.AddWithValue("@IsCommercial", comercialVehicle.IsCommercial);
                        cmd.Parameters.AddWithValue("@RollCage", comercialVehicle.RollCage);
                        cmd.Parameters.AddWithValue("@LoadCapacity", comercialVehicle.LoadCapacity);

                        int vehicleId = Convert.ToInt32(cmd.ExecuteScalar());
                        comercialVehicle.Id = vehicleId.ToString();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error adding comercial vehicle to the database: " + ex.Message);
                    }
                }
            }
        }

        public ComercialVehicle GetComercialVehicleById(int vehicleId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT v.*, nv.NormalVehicleId, nv.NumberOfSeats, nv.TrunkDimensions, nv.IsCommercial, cv.RollCage, cv.LoadCapacity
                    FROM Vehicles v
                    INNER JOIN NormalVehicles nv ON v.VehicleId = nv.VehicleId
                    INNER JOIN ComercialVehicles cv ON nv.NormalVehicleId = cv.NormalVehicleId
                    WHERE v.VehicleId = @VehicleId;";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@VehicleId", vehicleId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ComercialVehicle(
                            id: reader["VehicleId"].ToString(),
                            name: reader["Name"].ToString(),
                            km: reader["KM"].ToString(),
                            regristrationNumber: reader["RegistrationNumber"].ToString(),
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
    }
}
