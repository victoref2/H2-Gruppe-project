using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
         public void AddUser(User user)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            INSERT INTO Users (Name, PassWord, Mail) 
                            VALUES (@Name, @PassWord, @Mail);
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@Name", user.Name);
                        cmd.Parameters.AddWithValue("@PassWord", user.PassWord);
                        cmd.Parameters.AddWithValue("@Mail", user.Mail);

                        int userId = Convert.ToInt32(cmd.ExecuteScalar());
                        user.Id = userId.ToString();  

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error adding user to database: " + ex.Message);
                    }
                }
            }
        }

        public User GetUser(string userId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"SELECT * FROM Users WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                User user = null;

                if (reader.Read())
                {
                    user = new User(
                        id: reader["UserId"].ToString(),
                        name: reader["Name"].ToString(),
                        passWord: reader["PassWord"].ToString(),
                        mail: reader["Mail"].ToString()
                    );
                }

                return user;
            }
        }

        public User GetUserByEmail(string email)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"SELECT * FROM Users WHERE Mail = @Mail";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Mail", email);

                SqlDataReader reader = cmd.ExecuteReader();
                User user = null;

                if (reader.Read())
                {
                    user = new User(
                        id: reader["UserId"].ToString(),
                        name: reader["Name"].ToString(),
                        passWord: reader["PassWord"].ToString(),
                        mail: reader["Mail"].ToString()
                    );
                }
                return user;
            }
        }

        public void DeleteUser(string userId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = "DELETE FROM Users WHERE UserId = @UserId";
                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error deleting user from database: " + ex.Message);
                    }
                }
            }
        }
    }
}
