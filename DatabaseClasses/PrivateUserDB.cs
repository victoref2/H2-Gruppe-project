using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        // Create a Private User
        public void CreatePrivateUser(PrivateUser privateUser, int Id)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("CreatePrivateUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", Id);
                    cmd.Parameters.AddWithValue("@CPRNumber", privateUser.CPRNumber);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"SQL error occurred: {ex.Message}");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        // Get All Private Users
        public List<PrivateUser> GetAllPrivateUsers()
        {
            List<PrivateUser> privateUsers = new List<PrivateUser>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("GetAllPrivateUsers", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int privateUserId = reader.GetInt32(0); // PrivateUserId
                            int userId = reader.GetInt32(1);       // UserId
                            string cprNumber = reader.GetString(2); // CPRNumber

                            // You can fetch User details here if needed
                            // User user = GetUserById(userId); // Implement this if needed

                            privateUsers.Add(new PrivateUser(privateUserId, "DummyName", "DummyPass", "dummy@mail.com", 0, cprNumber));
                        }
                    }
                }
            }
            return privateUsers;
        }

        // Get a Private User by ID
        public PrivateUser GetPrivateUserById(int privateUserId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("GetPrivateUserById", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PrivateUserId", privateUserId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = reader.GetInt32(1); // UserId
                            string cprNumber = reader.GetString(2); // CPRNumber

                            // You can fetch User details here if needed
                            // User user = GetUserById(userId); // Implement this if needed

                            return new PrivateUser(privateUserId, "DummyName", "DummyPass", "dummy@mail.com", 0, cprNumber);
                        }
                    }
                }
            }
            return null; // Return null if no PrivateUser found
        }

        // Delete a Private User
        public void DeletePrivateUser(int privateUserId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("DeletePrivateUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PrivateUserId", privateUserId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"SQL error occurred: {ex.Message}");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        throw;
                    }
                }
            }
        }
    }
}
