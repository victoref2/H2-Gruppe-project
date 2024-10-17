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
                SqlCommand cmd = new SqlCommand("sp_GetUserAuctions", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SellerUserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var vehicle = GetVehicle(Convert.ToInt32(reader["VehicleId"]));
                    var seller = GetUserById(Convert.ToInt32(reader["SellerUserId"]));
                    User buyer = reader["BuyerUserId"] != DBNull.Value ? GetUserById(Convert.ToInt32(reader["BuyerUserId"])) : null;

                    var auction = new Auction(
                        id: Convert.ToInt32(reader["AuctionId"]),
                        vehicle: vehicle,
                        seller: seller,
                        currentPrice: Convert.ToDecimal(reader["Price"]),
                        closingDate: Convert.ToDateTime(reader["ClosingDate"]),
                        currentBuyer: buyer
                    );
                    auctions.Add(auction);
                }
            }
            return auctions;
        }

        public List<Auction> GetAllAuctions()
        {
            var auctions = new List<Auction>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("sp_GetAllAuctions", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var vehicle = GetVehicle(Convert.ToInt32(reader["VehicleId"]));
                    var seller = GetUserById(Convert.ToInt32(reader["SellerUserId"]));
                    User buyer = reader["BuyerUserId"] != DBNull.Value ? GetUserById(Convert.ToInt32(reader["BuyerUserId"])) : null;

                    var auction = new Auction(
                        id: Convert.ToInt32(reader["AuctionId"]),
                        vehicle: vehicle,
                        seller: seller,
                        currentPrice: Convert.ToDecimal(reader["Price"]),
                        closingDate: Convert.ToDateTime(reader["ClosingDate"]),
                        currentBuyer: buyer
                    );
                    auctions.Add(auction);
                }
            }
            return auctions;
        }

        public void AddAuction(Auction auction)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("sp_AddAuction", connection, transaction);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@VehicleId", auction.Vehicle.Id);
                        cmd.Parameters.AddWithValue("@SellerUserId", auction.Seller.Id);
                        cmd.Parameters.AddWithValue("@Price", auction.CurrentPrice);
                        cmd.Parameters.AddWithValue("@ClosingDate", auction.ClosingDate);
                        cmd.Parameters.AddWithValue("@BuyerUserId", auction.CurrentBuyer != null ? auction.CurrentBuyer.Id : (object)DBNull.Value);

                        auction.Id = Convert.ToInt32(cmd.ExecuteScalar());
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

        public void UpdateAuction(Auction auction)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("sp_UpdateAuction", connection, transaction);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@AuctionId", auction.Id);
                        cmd.Parameters.AddWithValue("@VehicleId", auction.Vehicle.Id);
                        cmd.Parameters.AddWithValue("@SellerUserId", auction.Seller.Id);
                        cmd.Parameters.AddWithValue("@Price", auction.CurrentPrice);
                        cmd.Parameters.AddWithValue("@ClosingDate", auction.ClosingDate);
                        cmd.Parameters.AddWithValue("@BuyerUserId", auction.CurrentBuyer != null ? auction.CurrentBuyer.Id : (object)DBNull.Value);

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

        public void DeleteAuction(int auctionId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("sp_DeleteAuction", connection, transaction);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
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

        public Auction GetAuction(int auctionId)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("sp_GetAuction", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AuctionId", auctionId);

                SqlDataReader reader = cmd.ExecuteReader();
                Auction auction = null;

                if (reader.Read())
                {
                    var vehicle = GetVehicle(Convert.ToInt32(reader["VehicleId"]));
                    var seller = GetUserById(Convert.ToInt32(reader["SellerUserId"]));
                    User buyer = reader["BuyerUserId"] != DBNull.Value ? GetUserById(Convert.ToInt32(reader["BuyerUserId"])) : null;

                    auction = new Auction(
                        id: Convert.ToInt32(reader["AuctionId"]),
                        vehicle: vehicle,
                        seller: seller,
                        currentPrice: Convert.ToDecimal(reader["Price"]),
                        closingDate: Convert.ToDateTime(reader["ClosingDate"]),
                        currentBuyer: buyer
                    );
                }

                return auction;
            }
        }
    }
}
