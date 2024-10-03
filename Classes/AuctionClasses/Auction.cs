using System;
using System.Collections.Generic;

namespace H2_Gruppe_project.Classes
{
    public class Auction
    {
        public string Id { get; set; } // Auction ID
        public Vehicle Vehicle { get; set; } // The vehicle being auctioned
        public User Seller { get; set; } // The seller
        public decimal MinimumPrice { get; set; } // Minimum price for the vehicle
        public List<decimal> Bids { get; private set; } // List of bids

        private Action<decimal> _notifySeller; // Notification method for the seller

        public Auction(string id, Vehicle vehicle, User seller, decimal minimumPrice)
        {
            Id = id;
            Vehicle = vehicle;
            Seller = seller;
            MinimumPrice = minimumPrice;
            Bids = new List<decimal>(); // Initialize the list of bids
        }

        // Method to set an auction (SætTilSalg)
        public void SætTilSalg(Vehicle vehicle, User seller, decimal minPris)
        {
            this.Vehicle = vehicle;
            this.Seller = seller;
            this.MinimumPrice = minPris;
            Console.WriteLine($"Auction set: Vehicle: {vehicle}, Seller: {seller}, Minimum price: {minPris}");
        }

        // Overloaded SætTilSalg method with notifications
        public void SætTilSalg(Vehicle vehicle, User seller, decimal minPris, Action<decimal> notifySeller)
        {
            SætTilSalg(vehicle, seller, minPris);
            this._notifySeller = notifySeller; // Set the notification method
        }

        // Method to receive a bid (ModtagBud)
        public bool ModtagBud(User buyer, string auctionNumber, decimal bid)
        {
            if (bid >= MinimumPrice && (Bids.Count == 0 || bid > Bids[^1]))
            {
                Bids.Add(bid); // Add the bid to the list
                Console.WriteLine($"Bid accepted: {bid} from {buyer.Name}");

                _notifySeller?.Invoke(bid); // Notify the seller if a notification method exists
                return true;
            }

            Console.WriteLine("Bid rejected: Lower than minimum price or current highest bid.");
            return false;
        }

        // Method to accept the highest bid (AccepterBud)
        public bool AccepterBud(User seller, string auctionNumber)
        {
            if (Bids.Count > 0 && seller == this.Seller)
            {
                decimal acceptedBid = Bids[^1]; // Get the highest bid
                Console.WriteLine($"Bid accepted: {acceptedBid} for Auction {auctionNumber}");

                // Here you can add logic to transfer the vehicle to the buyer and complete the sale
                return true;
            }

            Console.WriteLine("No bids or seller does not match.");
            return false;
        }

        // Find auction by ID (FindAuktionMedID)
        public static Auction FindAuktionMedID(List<Auction> auctions, string auctionId)
        {
            return auctions.Find(a => a.Id == auctionId); // Find auction by ID
        }
    }
}
