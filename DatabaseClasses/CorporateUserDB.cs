using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public void AddCorporateUser(CorporateUser corporateUser)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    using (SqlCommand cmd = new SqlCommand("AddCorporateUser", connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        cmd.Parameters.AddWithValue("@UserId", corporateUser.Id);
                        cmd.Parameters.AddWithValue("@Credit", corporateUser.Credit);
                        cmd.Parameters.AddWithValue("@CVRNumber", corporateUser.CVRNumber);

                        // Execute the query
                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error adding corporate user: " + ex.Message);
                }
            }
        }


        // Read - Get CorporateUser by ID
        public CorporateUser GetCorporateUser(int corporateUserId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("GetCorporateUserById", connection); // Use the correct stored procedure name
                cmd.CommandType = CommandType.StoredProcedure;

                // Add the parameter for the stored procedure
                cmd.Parameters.AddWithValue("@CorporateUserId", corporateUserId);

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
                        isCorp: Convert.ToBoolean(reader["CorporateUser"]), // This should now work
                        credit: Convert.ToDecimal(reader["Credit"]),
                        cvrNumber: reader["CVRNumber"].ToString()
                    );
                }
                return corporateUser;
            }
        }



        public void UpdateCorporateUser(CorporateUser corporateUser)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                using (SqlCommand cmd = new SqlCommand("UpdateCorporateUser", connection, transaction))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        cmd.Parameters.AddWithValue("@UserId", corporateUser.Id);
                        cmd.Parameters.AddWithValue("@UserName", corporateUser.Name);
                        cmd.Parameters.AddWithValue("@Password", corporateUser.PassWord);
                        cmd.Parameters.AddWithValue("@Mail", corporateUser.Mail);
                        cmd.Parameters.AddWithValue("@Balance", corporateUser.Balance);
                        cmd.Parameters.AddWithValue("@Credit", corporateUser.Credit);
                        cmd.Parameters.AddWithValue("@CVRNumber", corporateUser.CVRNumber);

                        // Execute the stored procedure
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

        public void DeleteCorporateUser(int userId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                using (SqlCommand cmd = new SqlCommand("DeleteCorporateUser", connection, transaction))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        // Execute the stored procedure
                        cmd.ExecuteNonQuery();
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
