using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {


        public void UpdateUserPassword(int userId, string newPassword)
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

        public void UpdateUserBalance(int userId, decimal newBalance)
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

        

        // Read - Get User by Email
        public User GetUserByEmail(string email)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

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
                    int userId = Convert.ToInt32(reader["UserId"]);  // Use int for UserId
                    if (reader["CVRNumber"] != DBNull.Value)
                    {
                        user = new CorporateUser(
                            id: userId,  // Use the integer UserId
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
                            id: userId,  // Use the integer UserId
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
        public void DeleteUser(int userId)
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
        // Read - Get User by ID
        public User GetUser(int userId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                // Fetch whether the user is a Corporate or Private user
                string query = @"
                    SELECT u.UserId, u.UserName, u.PassWord, u.Mail, u.Balance, 
                           p.CPRNumber, c.CVRNumber, c.Credit
                    FROM Users u
                    LEFT JOIN PrivateUsers p ON u.UserId = p.UserId
                    LEFT JOIN CorporateUsers c ON u.UserId = c.UserId
                    WHERE u.UserId = @UserId";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                User user = null;

                if (reader.Read())
                {
                    // Identify if the user is corporate or private and return appropriate object
                    if (reader["CVRNumber"] != DBNull.Value)
                    {
                        user = new CorporateUser(
                            id: userId,  // Use the integer UserId
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
                            id: userId,  // Use the integer UserId
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
    }
}
