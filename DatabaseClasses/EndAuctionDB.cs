using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.DatabaseClasses
{
    public partial class Database
    {
        public int CreateEndedAuction(Auction auction)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("CreateEndedAuction", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@VehicleId", auction.Vehicle.Id);
                    cmd.Parameters.AddWithValue("@SellerUserId", auction.Seller.Id);
                    cmd.Parameters.AddWithValue("@BuyerUserId", auction.CurrentBuyer?.Id ?? (object)DBNull.Value); // Nullable
                    cmd.Parameters.AddWithValue("@Price", auction.CurrentPrice);
                    cmd.Parameters.AddWithValue("@ClosingDate", auction.ClosingDate);

                    // Execute and get the inserted EndAuctionId
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public List<Auction> GetAllEndedAuctionsByBuyerId(int buyerId)
        {
            var endedAuctions = new List<Auction>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("GetAllEndedAuctionsByBuyerId", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BuyerUserId", buyerId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var vehicle = GetVehicle(Convert.ToInt32(reader["VehicleId"]));
                    var seller = GetUserById(Convert.ToInt32(reader["SellerUserId"]));
                    var buyer = GetUserById(Convert.ToInt32(reader["BuyerUserId"]));

                    var endedAuction = new Auction(
                        id: Convert.ToInt32(reader["EndAuctionId"]),
                        vehicle: vehicle,
                        seller: seller,
                        currentPrice: Convert.ToDecimal(reader["Price"]),
                        closingDate: Convert.ToDateTime(reader["ClosingDate"]),
                        currentBuyer: buyer
                    );
                    endedAuctions.Add(endedAuction);
                }
            }
            return endedAuctions;
        }


        public List<Auction> GetAllEndedAuctionsBySellerId(int sellerId)
        {
            var endedAuctions = new List<Auction>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("GetAllEndedAuctionsBySellerId", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SellerUserId", sellerId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var vehicle = GetVehicle(Convert.ToInt32(reader["VehicleId"]));
                    var seller = GetUserById(Convert.ToInt32(reader["SellerUserId"]));
                    User buyer = reader["BuyerUserId"] != DBNull.Value ? GetUserById(Convert.ToInt32(reader["BuyerUserId"])) : null;

                    var endedAuction = new Auction(
                        id: Convert.ToInt32(reader["EndAuctionId"]),
                        vehicle: vehicle,
                        seller: seller,
                        currentPrice: Convert.ToDecimal(reader["Price"]),
                        closingDate: Convert.ToDateTime(reader["ClosingDate"]),
                        currentBuyer: buyer
                    );
                    endedAuctions.Add(endedAuction);
                }
            }
            return endedAuctions;
        }



    }
}
