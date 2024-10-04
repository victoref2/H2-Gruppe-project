using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public List<Auction> GetUserAuctions(int userId)
        {
            var auctions = new List<Auction>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"
                    SELECT * FROM Auctions 
                    WHERE SellerUserId = @SellerUserId";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@SellerUserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var vehicle = GetVehicle(Convert.ToInt32(reader["VehicleId"]));  // Assuming GetVehicle method exists
                    var seller = GetUser(Convert.ToInt32(reader["SellerUserId"]));  // Seller
                    User buyer = null;

                    if (reader["BuyerUserId"] != DBNull.Value)
                    {
                        buyer = GetUser(Convert.ToInt32(reader["BuyerUserId"]));  // Buyer, if exists
                    }

                    var auction = new Auction(
                        id: reader["AuctionId"].ToString(),
                        vehicle: vehicle,
                        seller: seller,
                        currentPrice: Convert.ToDecimal(reader["Price"]),
                        closingDate: Convert.ToDateTime(reader["ClosingDate"]),
                        currentBuyer: buyer // Buyer, if exists
                    );

                    auctions.Add(auction);
                }
            }

            return auctions;
        }

        // Method to get all auctions in the system
        public List<Auction> GetAllAuctions()
        {
            var auctions = new List<Auction>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"SELECT * FROM Auctions";

                SqlCommand cmd = new SqlCommand(query, connection);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var vehicle = GetVehicle(Convert.ToInt32(reader["VehicleId"]));  // Assuming GetVehicle method exists
                    var seller = GetUser(Convert.ToInt32(reader["SellerUserId"]));  // Seller
                    User buyer = null;

                    if (reader["BuyerUserId"] != DBNull.Value)
                    {
                        buyer = GetUser(Convert.ToInt32(reader["BuyerUserId"]));  // Buyer, if exists
                    }

                    var auction = new Auction(
                        id: reader["AuctionId"].ToString(),
                        vehicle: vehicle,
                        seller: seller,
                        currentPrice: Convert.ToDecimal(reader["Price"]),
                        closingDate: Convert.ToDateTime(reader["ClosingDate"]),
                        currentBuyer: buyer // Buyer, if exists
                    );

                    auctions.Add(auction);
                }
            }

            return auctions;
        }

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
                            INSERT INTO Auctions (VehicleId, SellerUserId, Price, ClosingDate, BuyerUserId) 
                            VALUES (@VehicleId, @SellerUserId, @Price, @ClosingDate, @BuyerUserId);
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);

                        cmd.Parameters.AddWithValue("@VehicleId", auction.Vehicle.Id); // Vehicle ID
                        cmd.Parameters.AddWithValue("@SellerUserId", auction.Seller.Id); // Seller/User ID (now int)
                        cmd.Parameters.AddWithValue("@Price", auction.CurrentPrice); // Auction starting/current price
                        cmd.Parameters.AddWithValue("@ClosingDate", auction.ClosingDate); // Auction closing date
                        cmd.Parameters.AddWithValue("@BuyerUserId", auction.CurrentBuyer != null ? auction.CurrentBuyer.Id : (object)DBNull.Value); // Buyer ID (can be null)

                        int auctionId = Convert.ToInt32(cmd.ExecuteScalar());
                        auction.Id = auctionId.ToString(); // Store the Auction ID in the auction object

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
                                SellerUserId = @SellerUserId, 
                                Price = @Price, 
                                ClosingDate = @ClosingDate,
                                BuyerUserId = @BuyerUserId
                            WHERE AuctionId = @AuctionId";

                        SqlCommand cmd = new SqlCommand(query, connection, transaction);
                        cmd.Parameters.AddWithValue("@VehicleId", auction.Vehicle.Id);
                        cmd.Parameters.AddWithValue("@SellerUserId", auction.Seller.Id);  // Seller ID (now int)
                        cmd.Parameters.AddWithValue("@Price", auction.CurrentPrice);  // Current price
                        cmd.Parameters.AddWithValue("@ClosingDate", auction.ClosingDate);  // Closing date
                        cmd.Parameters.AddWithValue("@BuyerUserId", auction.CurrentBuyer != null ? auction.CurrentBuyer.Id : (object)DBNull.Value); // Buyer ID (can be null)
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
