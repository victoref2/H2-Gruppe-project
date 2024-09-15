using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H2_Gruppe_project.Classes;
using Tmds.DBus.Protocol;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public void AddVehicle(Vehicle vehicle) {

            using (SqlConnection connection = GetConnection()) {
                connection.Open();
                string query = @"INSERT INTO Vehicles (Id, Name, Km, RegistrationNumber, AgeGroup, TowHook, DriversLicenceClass, EngineSize, KmL, FuelType, EnergyClass) 
                                 VALUES (@Id, @Name, @Km, @RegistrationNumber, @AgeGroup, @TowHook, @DriversLicenceClass, @EngineSize, @KmL, @FuelType, @EnergyClass)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", vehicle.Id);
                cmd.Parameters.AddWithValue("@Name", vehicle.Name);
                cmd.Parameters.AddWithValue("@Km", vehicle.KM);
                cmd.Parameters.AddWithValue("@RegistrationNumber", vehicle.RegristrationNumber);
                cmd.Parameters.AddWithValue("@AgeGroup", vehicle.AgeGroup);
                cmd.Parameters.AddWithValue("@TowHook", vehicle.TowHook);
                cmd.Parameters.AddWithValue("@DriversLicenceClass", vehicle.DriversLicenceClass);
                cmd.Parameters.AddWithValue("@EngineSize", vehicle.EngineSize);
                cmd.Parameters.AddWithValue("@KmL", vehicle.KmL);
                cmd.Parameters.AddWithValue("@FuelType", vehicle.FuelType);
                cmd.Parameters.AddWithValue("@EnergyClass", vehicle.EnergyClass);

                cmd.ExecuteNonQuery();
            }
        }

        public Vehicle GetVehicle(string vehicleId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"SELECT * FROM Vehicles WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", vehicleId);

                SqlDataReader reader = cmd.ExecuteReader();
                Vehicle vehicle = null;

                if (reader.Read())
                {
                    vehicle = new Vehicle(
                        reader["Id"].ToString(),
                        reader["Name"].ToString(),
                        reader["Km"].ToString(),
                        reader["RegistrationNumber"].ToString(),
                        reader["AgeGroup"].ToString(),
                        Convert.ToBoolean(reader["TowHook"]),
                        reader["DriversLicenceClass"].ToString(),
                        reader["EngineSize"].ToString(),
                        Convert.ToDecimal(reader["KmL"]),
                        reader["FuelType"].ToString(),
                        reader["EnergyClass"].ToString()
                    );
                }

                return vehicle;
            }
        }

        public void DeleteVehicle(string vehicleId){
            using (SqlConnection connection = GetConnection()){

                connection.Open();
                string query = "DELETE FROM Vehicles WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", vehicleId);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
