using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        //  public void AddUser(User user)
        // {
        //     using (SqlConnection connection = GetConnection())
        //     {
        //         connection.Open();
        //         using (SqlTransaction transaction = connection.BeginTransaction())
        //         {
        //             try
        //             {
        //                 string query = @"
        //                     INSERT INTO Users (Name, PassWord, Mail) 
        //                     VALUES (@Name, @PassWord, @Mail);
        //                     SELECT SCOPE_IDENTITY();";

        //                 SqlCommand cmd = new SqlCommand(query, connection, transaction);
        //                 cmd.Parameters.AddWithValue("@Name", user.Name);
        //                 cmd.Parameters.AddWithValue("@PassWord", user.PassWord);
        //                 cmd.Parameters.AddWithValue("@Mail", user.Mail);

        //                 int userId = Convert.ToInt32(cmd.ExecuteScalar());
        //                 user.Id = userId.ToString();  

        //                 transaction.Commit();
        //             }
        //             catch (Exception ex)
        //             {
        //                 transaction.Rollback();
        //                 throw new Exception("Error adding user to database: " + ex.Message);
        //             }
        //         }
        //     }
        // }

        // public User GetUser(string userId)
        // {
        //     using (SqlConnection connection = GetConnection())
        //     {
        //         connection.Open();

        //         string query = @"SELECT * FROM Users WHERE UserId = @UserId";
        //         SqlCommand cmd = new SqlCommand(query, connection);
        //         cmd.Parameters.AddWithValue("@UserId", userId);

        //         SqlDataReader reader = cmd.ExecuteReader();
        //         User user = null;

        //         if (reader.Read())
        //         {
        //             user = new User(
        //                 id: reader["UserId"].ToString(),
        //                 name: reader["Name"].ToString(),
        //                 passWord: reader["PassWord"].ToString(),
        //                 mail: reader["Mail"].ToString()
        //             );
        //         }

        //         return user;
        //     }
        // }

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

        // public void DeleteUser(string userId)
        // {
        //     using (SqlConnection connection = GetConnection())
        //     {
        //         connection.Open();
        //         using (SqlTransaction transaction = connection.BeginTransaction())
        //         {
        //             try
        //             {
        //                 string query = "DELETE FROM Users WHERE UserId = @UserId";
        //                 SqlCommand cmd = new SqlCommand(query, connection, transaction);
        //                 cmd.Parameters.AddWithValue("@UserId", userId);

        //                 cmd.ExecuteNonQuery();

        //                 transaction.Commit();
        //             }
        //             catch (Exception ex)
        //             {
        //                 transaction.Rollback();
        //                 throw new Exception("Error deleting user from database: " + ex.Message);
        //             }
        //         }
        //     }
        // }
    }
}