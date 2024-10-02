using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        // Create - Add User
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
                            INSERT INTO Users (UserName, PassWord, Mail, Balance) 
                            VALUES (@Name, @PassWord, @Mail, @Balance);
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@Name", user.Name);
                        cmd.Parameters.AddWithValue("@PassWord", user.PassWord);
                        cmd.Parameters.AddWithValue("@Mail", user.Mail);
                        cmd.Parameters.AddWithValue("@Balance", user.Balance);

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

        public void UpdateUserPassword(string userId, string newPassword)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            UPDATE Users
                            SET PassWord = @PassWord
                            WHERE UserId = @UserId";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);

                        string hashedPassword = User.HashPassword(newPassword);
                        cmd.Parameters.AddWithValue("@PassWord", hashedPassword);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error updating user password: " + ex.Message);
                    }
                }
            }
        }

        public void UpdateUserBalance(string userId, decimal newBalance)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            UPDATE Users
                            SET Balance = @Balance
                            WHERE UserId = @UserId";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@Balance", newBalance);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error updating user balance: " + ex.Message);
                    }
                }
            }
        }



        // Read - Get User by UserId
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
                        name: reader["UserName"].ToString(),
                        passWord: reader["PassWord"].ToString(),
                        mail: reader["Mail"].ToString(),
                        balance: Convert.ToDecimal(reader["Balance"])
                    );
                }

                return user;
            }
        }

        // Read - Get User by Email (Already implemented)
        public User GetUserByEmail(string email)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                // Fetch whether the user is a Corporate or Private user
                string query = @"SELECT u.UserId, u.UserName, u.PassWord, u.Mail, u.Balance, 
                                        p.CPRNumber, c.CVRNumber, c.Credit
                                FROM Users u
                                LEFT JOIN PrivateUsers p ON u.UserId = p.UserId
                                LEFT JOIN CorporateUsers c ON u.UserId = c.UserId
                                WHERE u.Mail = @Mail";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Mail", email);

                SqlDataReader reader = cmd.ExecuteReader();
                User user = null;

                if (reader.Read())
                {
                    if (reader["CVRNumber"] != DBNull.Value)
                    {
                        user = new CorporateUser(
                            id: reader["UserId"].ToString(),
                            name: reader["UserName"].ToString(),
                            passWord: reader["PassWord"].ToString(),
                            mail: reader["Mail"].ToString(),
                            balance: Convert.ToDecimal(reader["Balance"]),
                            credit: Convert.ToDecimal(reader["Credit"]),
                            cvrNumber: reader["CVRNumber"].ToString()
                        );
                    }
                    else if (reader["CPRNumber"] != DBNull.Value)
                    {
                        user = new PrivateUser(
                            id: reader["UserId"].ToString(),
                            name: reader["UserName"].ToString(),
                            passWord: reader["PassWord"].ToString(),
                            mail: reader["Mail"].ToString(),
                            balance: Convert.ToDecimal(reader["Balance"]),
                            cprNumber: reader["CPRNumber"].ToString()
                        );
                    }
                }
                return user;
            }
        }

        // Delete - Delete User by UserId
        public void DeleteUser(string userId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // First, delete from PrivateUsers or CorporateUsers (if applicable)
                        string deleteCorporateUserQuery = "DELETE FROM CorporateUsers WHERE UserId = @UserId";
                        string deletePrivateUserQuery = "DELETE FROM PrivateUsers WHERE UserId = @UserId";

                        SqlCommand deleteCorporateUserCmd = new SqlCommand(deleteCorporateUserQuery, connection, transaction);
                        SqlCommand deletePrivateUserCmd = new SqlCommand(deletePrivateUserQuery, connection, transaction);

                        deleteCorporateUserCmd.Parameters.AddWithValue("@UserId", userId);
                        deletePrivateUserCmd.Parameters.AddWithValue("@UserId", userId);

                        // Try to delete in both tables (only one will succeed)
                        deleteCorporateUserCmd.ExecuteNonQuery();
                        deletePrivateUserCmd.ExecuteNonQuery();

                        // Finally, delete from Users table
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
