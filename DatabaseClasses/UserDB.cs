using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using H2_Gruppe_project.Classes;
using Tmds.DBus.Protocol;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public int CreateUser(User user, bool corporateUser)
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    int Id;

                    using (SqlCommand cmd = new SqlCommand("CreateAUser", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@LoginName", user.Name);
                        cmd.Parameters.AddWithValue("@Password", user.PassWord);

                        cmd.ExecuteNonQuery();
                    }
                    // Step 1: Insert the user into the Users table
                    using (SqlCommand cmd = new SqlCommand("CreateUser", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserName", user.Name);
                        cmd.Parameters.AddWithValue("@Password", user.PassWord);
                        cmd.Parameters.AddWithValue("@CorporateUser", corporateUser);
                        cmd.Parameters.AddWithValue("@Balance", user.Balance);
                        cmd.Parameters.AddWithValue("@Mail", user.Mail);

                        return Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("GetAllUsers", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User
                    (
                        (int)reader["UserId"],
                        reader["UserName"].ToString(),
                        null,//no password for you
                        reader["Mail"].ToString(),
                        (decimal)reader["Balance"]
                    );
                    users.Add(user);
                }
                connection.Close();
            }
            return users;
        }

        public User GetUserById(int userId)
        {
            User user = null;

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("GetUserById", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new User
                    (
                        (int)reader["UserId"],
                        reader["UserName"].ToString(),
                        null, // Password is not retrieved
                        reader["Mail"].ToString(),
                        (decimal)reader["Balance"]
                    );
                }
                connection.Close();
            }

            return user;
        }

        public User GetUserByName(string userName)
        {
            User user = null;

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("GetUserByName", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", userName);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new User
                    (
                        (int)reader["UserId"],
                        reader["UserName"].ToString(),
                        reader["PassWord"].ToString(),
                        reader["Mail"].ToString(),
                        (decimal)reader["Balance"]
                    );
                }
                connection.Close();
            }

            return user;
        }

        public void UpdateUser(User user, bool corporateUser)
        {
            // Ensure user object is not null before proceeding
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            // Establish database connection
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("UpdateUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the command
                    cmd.Parameters.AddWithValue("@UserId", user.Id);
                    cmd.Parameters.AddWithValue("@UserName", user.Name);
                    cmd.Parameters.AddWithValue("@Password", user.PassWord);
                    cmd.Parameters.AddWithValue("@CorporateUser", corporateUser);
                    cmd.Parameters.AddWithValue("@Balance", user.Balance);
                    cmd.Parameters.AddWithValue("@Mail", user.Mail);

                    // Execute the stored procedure
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
                connection.Close();
            }
        }

        public void DeleteUser(int userId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("DeleteUser", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);

                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void UpdateUserBalance(int userId, decimal newBalance)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("UpdateUserBalance", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@NewBalance", newBalance);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"SQL error occurred: {ex.Message}");
                        throw; // Rethrow the exception to handle it at a higher level if needed
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        throw; // Rethrow for handling elsewhere
                    }
                }
            }

        }

        public void UpdateUserPassword(int userId, string newPassword, string Name)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("UpdateUserPassword", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@NewPassword", newPassword); // Hash the new password
                    cmd.Parameters.AddWithValue("@UserName", Name);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"SQL error occurred: {ex.Message}");
                        throw; // Rethrow the exception to handle it at a higher level if needed
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        throw; // Rethrow for handling elsewhere
                    }
                }
            }
        }
    }
}