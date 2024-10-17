using System;
using System.Data.SqlClient;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        private static SqlConnection GetConnection()
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder
            {
                DataSource = "localhost", 
                InitialCatalog = "AutoAuctionDB",
                UserID = "AddsUsers_User",
                Password = "Password123!"
            };

            SqlConnection connection = new SqlConnection(sb.ToString());
            return connection;
        }

        // Test method to verify the database connection
        public static void TestConnection()
        {
            using (SqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection to the database was successful.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to connect to the database: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
