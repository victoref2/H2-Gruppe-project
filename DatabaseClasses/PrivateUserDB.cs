using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
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
                        privateUser.Id = userId.ToString();

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
                        id: reader["UserId"].ToString(),
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

    }
}