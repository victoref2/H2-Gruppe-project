using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        // Create - Add PrivateUser
        public void AddPrivateUser(PrivateUser privateUser)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            EXEC AddPrivateUser @UserName, @Password, @Mail, @CPRNumber, @Balance;
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);

                        cmd.Parameters.AddWithValue("@UserName", privateUser.Name);
                        cmd.Parameters.AddWithValue("@Password", privateUser.PassWord);
                        cmd.Parameters.AddWithValue("@Mail", privateUser.Mail);
                        cmd.Parameters.AddWithValue("@CPRNumber", privateUser.CPRNumber);
                        cmd.Parameters.AddWithValue("@Balance", privateUser.Balance);

                        int userId = Convert.ToInt32(cmd.ExecuteScalar());
                        privateUser.Id = userId;

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error adding private user to database: " + ex.Message);
                    }
                }
            }
        }

        // Read - Get PrivateUser by ID
        public PrivateUser GetPrivateUser(int userId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"SELECT u.UserId, u.UserName, u.Password, u.Mail, u.Balance, p.CPRNumber 
                                FROM Users u 
                                JOIN PrivateUsers p ON u.UserId = p.UserId 
                                WHERE u.UserId = @UserId";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                PrivateUser privateUser = null;

                if (reader.Read())
                {
                    privateUser = new PrivateUser(
                        id: Convert.ToInt32(reader["UserId"]),
                        name: reader["UserName"].ToString(),
                        passWord: reader["Password"].ToString(),
                        mail: reader["Mail"].ToString(),
                        balance: Convert.ToDecimal(reader["Balance"]),
                        cprNumber: reader["CPRNumber"].ToString()
                    );
                }
                return privateUser;
            }
        }

        // Update - Update PrivateUser
        public void UpdatePrivateUser(PrivateUser privateUser)
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
                            SET UserName = @UserName, Password = @Password, Mail = @Mail, Balance = @Balance
                            WHERE UserId = @UserId;

                            UPDATE PrivateUsers
                            SET CPRNumber = @CPRNumber
                            WHERE UserId = @UserId;";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@UserName", privateUser.Name);
                        cmd.Parameters.AddWithValue("@Password", privateUser.PassWord);
                        cmd.Parameters.AddWithValue("@Mail", privateUser.Mail);
                        cmd.Parameters.AddWithValue("@Balance", privateUser.Balance);
                        cmd.Parameters.AddWithValue("@CPRNumber", privateUser.CPRNumber);
                        cmd.Parameters.AddWithValue("@UserId", privateUser.Id);

                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error updating private user in database: " + ex.Message);
                    }
                }
            }
        }

        // Delete - Delete PrivateUser by ID
        public void DeletePrivateUser(int userId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // First, delete the record from PrivateUsers table
                        string deletePrivateUserQuery = "DELETE FROM PrivateUsers WHERE UserId = @UserId;";
                        SqlCommand deletePrivateUserCmd = new SqlCommand(deletePrivateUserQuery, connection, transaction);
                        deletePrivateUserCmd.Parameters.AddWithValue("@UserId", userId);
                        deletePrivateUserCmd.ExecuteNonQuery();

                        // Then, delete the record from Users table
                        string deleteUserQuery = "DELETE FROM Users WHERE UserId = @UserId;";
                        SqlCommand deleteUserCmd = new SqlCommand(deleteUserQuery, connection, transaction);
                        deleteUserCmd.Parameters.AddWithValue("@UserId", userId);
                        deleteUserCmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error deleting private user from database: " + ex.Message);
                    }
                }
            }
        }
    }
}
