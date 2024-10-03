using System;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        // Create - Add Auction
        public void AddAuction(Auction auction)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            INSERT INTO Auctions (VehicleId, UserId, Price) 
                            VALUES (@VehicleId, @UserId, @Price);
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@VehicleId", auction.Vehicle.Id);
                        cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(auction.Seller.Id));
                        cmd.Parameters.AddWithValue("@Price", auction.CurrentPrice);

                        int auctionId = Convert.ToInt32(cmd.ExecuteScalar());
                        auction.Id = auctionId.ToString();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error adding auction to database: " + ex.Message);
                    }
                }
            }
        }

        // Read - Get Auction by ID
        public Auction GetAuction(int auctionId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"SELECT * FROM Auctions WHERE AuctionId = @AuctionId";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@AuctionId", auctionId);

                SqlDataReader reader = cmd.ExecuteReader();
                Auction auction = null;

                if (reader.Read())
                {
                    var vehicle = GetVehicle(Convert.ToInt32(reader["VehicleId"]));
                    var user = GetUser(reader["UserId"].ToString());

                    /*auction = new Auction(
                        id: reader["AuctionId"].ToString(),
                        vehicle: vehicle,
                        user: user,
                        price: Convert.ToDecimal(reader["Price"])
                    );*/
                }
                return auction;
            }
        }

        // Update - Update Auction
        public void UpdateAuction(Auction auction)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            UPDATE Auctions 
                            SET VehicleId = @VehicleId, 
                                UserId = @UserId, 
                                Price = @Price 
                            WHERE AuctionId = @AuctionId";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@VehicleId", auction.Vehicle.Id);
                        cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(auction.Seller.Id));
                        cmd.Parameters.AddWithValue("@Price", auction.CurrentBuyer);
                        cmd.Parameters.AddWithValue("@AuctionId", auction.Id);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error updating auction in database: " + ex.Message);
                    }
                }
            }
        }

        // Delete - Delete Auction
        public void DeleteAuction(int auctionId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = "DELETE FROM Auctions WHERE AuctionId = @AuctionId";
                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@AuctionId", auctionId);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error deleting auction from database: " + ex.Message);
                    }
                }
            }
        }
    }
}
