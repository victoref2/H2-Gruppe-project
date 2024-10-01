using System;
using System.Data.SqlClient;
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
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            EXEC AddCorporateUser @UserName, @Password, @Mail, @Credit, @CVRNumber;
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);

                        cmd.Parameters.AddWithValue("@UserName", corporateUser.Name);
                        cmd.Parameters.AddWithValue("@Password", corporateUser.PassWord);
                        cmd.Parameters.AddWithValue("@Mail", corporateUser.Mail);
                        cmd.Parameters.AddWithValue("@Credit", corporateUser.Credit);
                        cmd.Parameters.AddWithValue("@CVRNumber", corporateUser.CVRNumber);

                        int userId = Convert.ToInt32(cmd.ExecuteScalar());
                        corporateUser.Id = userId.ToString();

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
                        id: reader["UserId"].ToString(),
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
    }
}
