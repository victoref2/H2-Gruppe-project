using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        // Create - Add NormalVehicle
        public void AddNormalVehicle(NormalVehicle normalVehicle)
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
                        vehicleCmd.Parameters.AddWithValue("@Name", normalVehicle.Name);
                        vehicleCmd.Parameters.AddWithValue("@KM", normalVehicle.KM);
                        vehicleCmd.Parameters.AddWithValue("@RegistrationNumber", normalVehicle.RegristrationNumber);
                        vehicleCmd.Parameters.AddWithValue("@AgeGroup", normalVehicle.AgeGroup);
                        vehicleCmd.Parameters.AddWithValue("@TowHook", normalVehicle.TowHook);
                        vehicleCmd.Parameters.AddWithValue("@DriversLicenceClass", normalVehicle.DriversLicenceClass);
                        vehicleCmd.Parameters.AddWithValue("@EngineSize", decimal.Parse(normalVehicle.EngineSize.TrimEnd('L', 'l')));
                        vehicleCmd.Parameters.AddWithValue("@KmL", normalVehicle.KmL);
                        vehicleCmd.Parameters.AddWithValue("@FuelType", normalVehicle.FuelType);
                        vehicleCmd.Parameters.AddWithValue("@EnergyClass", normalVehicle.EnergyClass);

                        int vehicleId = Convert.ToInt32(vehicleCmd.ExecuteScalar());

                        // Insert into NormalVehicles table
                        string normalVehicleQuery = @"
                            INSERT INTO NormalVehicles (VehicleId, NumberOfSeats, TrunkDimensions, IsCommercial)
                            VALUES (@VehicleId, @NumberOfSeats, @TrunkDimensions, @IsCommercial);
                            SELECT SCOPE_IDENTITY();";
                        SqlCommand normalVehicleCmd = new SqlCommand(normalVehicleQuery, connection, transaction);
                        normalVehicleCmd.Parameters.AddWithValue("@VehicleId", vehicleId);
                        normalVehicleCmd.Parameters.AddWithValue("@NumberOfSeats", normalVehicle.NumberOfSeats);
                        normalVehicleCmd.Parameters.AddWithValue("@TrunkDimensions", normalVehicle.TrunkDimensions);
                        normalVehicleCmd.Parameters.AddWithValue("@IsCommercial", normalVehicle.IsCommercial);

                        int normalVehicleId = Convert.ToInt32(normalVehicleCmd.ExecuteScalar());

                        normalVehicle.NormalVehicleId = normalVehicleId;
                        normalVehicle.Id = vehicleId.ToString();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error adding normal vehicle to database: " + ex.Message);
                    }
                }
            }
        }

        // Read - Get NormalVehicle by ID
        public NormalVehicle GetNormalVehicleById(int normalVehicleId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT v.*, nv.NormalVehicleId, nv.NumberOfSeats, nv.TrunkDimensions, nv.IsCommercial
                    FROM NormalVehicles nv
                    INNER JOIN Vehicles v ON nv.VehicleId = v.VehicleId
                    WHERE nv.NormalVehicleId = @NormalVehicleId;";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@NormalVehicleId", normalVehicleId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new NormalVehicle(
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
                            isCommercial: Convert.ToBoolean(reader["IsCommercial"])
                        )
                        {
                            NormalVehicleId = normalVehicleId
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        // Update - Update NormalVehicle
        public void UpdateNormalVehicle(NormalVehicle normalVehicle)
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
                                TowHook = @TowHook, DriversLicenceClass = @DriversLicenceClass, EngineSize = @EngineSize, KmL = @KmL, 
                                FuelType = @FuelType, EnergyClass = @EnergyClass
                            WHERE VehicleId = (SELECT VehicleId FROM NormalVehicles WHERE NormalVehicleId = @NormalVehicleId)";
                        SqlCommand vehicleCmd = new SqlCommand(vehicleQuery, connection, transaction);
                        vehicleCmd.Parameters.AddWithValue("@Name", normalVehicle.Name);
                        vehicleCmd.Parameters.AddWithValue("@KM", normalVehicle.KM);
                        vehicleCmd.Parameters.AddWithValue("@RegistrationNumber", normalVehicle.RegristrationNumber);
                        vehicleCmd.Parameters.AddWithValue("@AgeGroup", normalVehicle.AgeGroup);
                        vehicleCmd.Parameters.AddWithValue("@TowHook", normalVehicle.TowHook);
                        vehicleCmd.Parameters.AddWithValue("@DriversLicenceClass", normalVehicle.DriversLicenceClass);
                        vehicleCmd.Parameters.AddWithValue("@EngineSize", decimal.Parse(normalVehicle.EngineSize.TrimEnd('L', 'l')));
                        vehicleCmd.Parameters.AddWithValue("@KmL", normalVehicle.KmL);
                        vehicleCmd.Parameters.AddWithValue("@FuelType", normalVehicle.FuelType);
                        vehicleCmd.Parameters.AddWithValue("@EnergyClass", normalVehicle.EnergyClass);
                        vehicleCmd.Parameters.AddWithValue("@NormalVehicleId", normalVehicle.NormalVehicleId);

                        vehicleCmd.ExecuteNonQuery();

                        // Update NormalVehicles table
                        string normalVehicleQuery = @"
                            UPDATE NormalVehicles
                            SET NumberOfSeats = @NumberOfSeats, TrunkDimensions = @TrunkDimensions, IsCommercial = @IsCommercial
                            WHERE NormalVehicleId = @NormalVehicleId";
                        SqlCommand normalVehicleCmd = new SqlCommand(normalVehicleQuery, connection, transaction);
                        normalVehicleCmd.Parameters.AddWithValue("@NumberOfSeats", normalVehicle.NumberOfSeats);
                        normalVehicleCmd.Parameters.AddWithValue("@TrunkDimensions", normalVehicle.TrunkDimensions);
                        normalVehicleCmd.Parameters.AddWithValue("@IsCommercial", normalVehicle.IsCommercial);
                        normalVehicleCmd.Parameters.AddWithValue("@NormalVehicleId", normalVehicle.NormalVehicleId);

                        normalVehicleCmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error updating normal vehicle in database: " + ex.Message);
                    }
                }
            }
        }

        // Delete - Delete NormalVehicle by ID
        public void DeleteNormalVehicle(int normalVehicleId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Retrieve VehicleId first
                        string getVehicleIdQuery = "SELECT VehicleId FROM NormalVehicles WHERE NormalVehicleId = @NormalVehicleId;";
                        SqlCommand getVehicleIdCmd = new SqlCommand(getVehicleIdQuery, connection, transaction);
                        getVehicleIdCmd.Parameters.AddWithValue("@NormalVehicleId", normalVehicleId);

                        int vehicleId = (int)getVehicleIdCmd.ExecuteScalar();

                        // Delete from NormalVehicles table
                        string deleteNormalVehicleQuery = "DELETE FROM NormalVehicles WHERE NormalVehicleId = @NormalVehicleId;";
                        SqlCommand deleteNormalVehicleCmd = new SqlCommand(deleteNormalVehicleQuery, connection, transaction);
                        deleteNormalVehicleCmd.Parameters.AddWithValue("@NormalVehicleId", normalVehicleId);
                        deleteNormalVehicleCmd.ExecuteNonQuery();

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
                        throw new Exception("Error deleting normal vehicle from database: " + ex.Message);
                    }
                }
            }
        }
    }
}
