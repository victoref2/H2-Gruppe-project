using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        // Create - Add CorporateUser
        public void AddCorporateUser(CorporateUser corporateUser)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            EXEC AddCorporateUser @UserName, @Password, @Mail, @Credit, @CVRNumber, @Balance;
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);

                        cmd.Parameters.AddWithValue("@UserName", corporateUser.Name);
                        cmd.Parameters.AddWithValue("@Password", corporateUser.PassWord);
                        cmd.Parameters.AddWithValue("@Mail", corporateUser.Mail);
                        cmd.Parameters.AddWithValue("@Credit", corporateUser.Credit);
                        cmd.Parameters.AddWithValue("@CVRNumber", corporateUser.CVRNumber);
                        cmd.Parameters.AddWithValue("@Balance", corporateUser.Balance);

                        int userId = Convert.ToInt32(cmd.ExecuteScalar());
                        corporateUser.Id = userId;

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error adding corporate user to database: " + ex.Message);
                    }
                }
            }
        }

        // Read - Get CorporateUser by ID
        public CorporateUser GetCorporateUser(int userId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"
                    SELECT u.UserId, u.UserName, u.Password, u.Mail, u.Balance, 
                        c.Credit, c.CVRNumber 
                    FROM Users u 
                    JOIN CorporateUsers c ON u.UserId = c.UserId 
                    WHERE u.UserId = @UserId";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                CorporateUser corporateUser = null;

                if (reader.Read())
                {
                    corporateUser = new CorporateUser(
                        id: Convert.ToInt32(reader["UserId"]),
                        name: reader["UserName"].ToString(),
                        passWord: reader["Password"].ToString(),
                        mail: reader["Mail"].ToString(),
                        balance: Convert.ToDecimal(reader["Balance"]),
                        credit: Convert.ToDecimal(reader["Credit"]),
                        cvrNumber: reader["CVRNumber"].ToString()
                    );
                }
                return corporateUser;
            }
        }

        // Update - Update CorporateUser
        public void UpdateCorporateUser(CorporateUser corporateUser)
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

                            UPDATE CorporateUsers
                            SET Credit = @Credit, CVRNumber = @CVRNumber
                            WHERE UserId = @UserId;";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@UserName", corporateUser.Name);
                        cmd.Parameters.AddWithValue("@Password", corporateUser.PassWord);
                        cmd.Parameters.AddWithValue("@Mail", corporateUser.Mail);
                        cmd.Parameters.AddWithValue("@Balance", corporateUser.Balance);
                        cmd.Parameters.AddWithValue("@Credit", corporateUser.Credit);
                        cmd.Parameters.AddWithValue("@CVRNumber", corporateUser.CVRNumber);
                        cmd.Parameters.AddWithValue("@UserId", corporateUser.Id);

                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error updating corporate user in the database: " + ex.Message);
                    }
                }
            }
        }

        // Delete - Delete CorporateUser by ID
        public void DeleteCorporateUser(int userId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // First, delete the corporate user record from the CorporateUsers table
                        string query = "DELETE FROM CorporateUsers WHERE UserId = @UserId;";
                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.ExecuteNonQuery();

                        // Then, delete the user record from the Users table
                        string deleteUserQuery = "DELETE FROM Users WHERE UserId = @UserId;";
                        SqlCommand deleteUserCmd = new SqlCommand(deleteUserQuery, connection, transaction);
                        deleteUserCmd.Parameters.AddWithValue("@UserId", userId);
                        deleteUserCmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error deleting corporate user from the database: " + ex.Message);
                    }
                }
            }
        }
    }
}
