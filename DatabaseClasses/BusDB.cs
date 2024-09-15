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
        public void AddBus(Bus bus)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = @"INSERT INTO Bus (Id, Height, Weight, Length, NumberOfSeats, NumberOfSleepingPlaces, HasToilet)
                                VALUES (@Id, @Height, @Weight, @Length, @NumberOfSeats, @NumberOfSleepingPlaces, @HasToilet)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", bus.Id);
                cmd.Parameters.AddWithValue("@Height", bus.Height);
                cmd.Parameters.AddWithValue("@Weight", bus.Weight);
                cmd.Parameters.AddWithValue("@Length", bus.Length);
                cmd.Parameters.AddWithValue("@NumberOfSeats", bus.NumberOfSeats);
                cmd.Parameters.AddWithValue("@NumberOfSleepingPlaces", bus.NumberOfSleepingPlaces);
                cmd.Parameters.AddWithValue("@HasToilet", bus.HasToilet);

                cmd.ExecuteNonQuery();
            }
        }

        public Bus GetBusById(string busId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = @"SELECT * FROM Bus WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", busId);

                SqlDataReader reader = cmd.ExecuteReader();
                Bus bus = null;

                if (reader.Read())
                {
                    bus = new Bus(
                        reader["Id"].ToString(),
                        reader["Name"].ToString(),
                        reader["Km"].ToString(),
                        reader["RegistrationNumber"].ToString(),
                        reader["AgeGroup"].ToString(),
                        Convert.ToBoolean(reader["TowHook"]),
                        reader["EngineSize"].ToString(),
                        Convert.ToDecimal(reader["KmL"]),
                        reader["FuelType"].ToString(),
                        reader["EnergyClass"].ToString(),
                        Convert.ToInt32(reader["MaxLoadCapacity"]),
                        Convert.ToInt32(reader["NumberOfAxles"]),
                        Convert.ToDecimal(reader["Height"]),
                        Convert.ToDecimal(reader["Weight"]),
                        Convert.ToDecimal(reader["Length"]),
                        Convert.ToInt32(reader["NumberOfSeats"]),
                        Convert.ToInt32(reader["NumberOfSleepingPlaces"]),
                        Convert.ToBoolean(reader["HasToilet"])
                    );
                }

                return bus;
            }
        }

        public void DeleteBus(string busId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = @"DELETE FROM Bus WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", busId);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
