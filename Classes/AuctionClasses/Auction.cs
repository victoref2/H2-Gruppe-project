using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes
{
    public class Auction
    {
        public string Id { get; set; } // Auction ID
        public Vehicle Vehicle { get; set; } // The vehicle being auctioned
        public User Seller { get; set; } // The seller
        public User CurrentBuyer { get; set; } // The current highest bidder (user)
        public decimal CurrentPrice { get; set; } // The current highest bid (price)
        public List<decimal> Bids { get; set; } // List of bids
        public DateTime ClosingDate { get; set; } // Auction closing date

        private readonly object _bidLock = new object(); // Lock for bid modification

        // Constructor without the Buyer initially (before any bids)
        public Auction(string id, Vehicle vehicle, User seller, decimal currentPrice, DateTime closingDate)
        {
            Id = id;
            Vehicle = vehicle;
            Seller = seller;
            CurrentPrice = currentPrice;
            Bids = new List<decimal>(); // Initialize the list of bids
            ClosingDate = closingDate;
        }

        // Constructor with Buyer (for auctions that already have a buyer)
        public Auction(string id, Vehicle vehicle, User seller, decimal currentPrice, DateTime closingDate, User currentBuyer)
        {
            Id = id;
            Vehicle = vehicle;
            Seller = seller;
            CurrentPrice = currentPrice;
            ClosingDate = closingDate;
            CurrentBuyer = currentBuyer; // Set the current buyer (if already exists)
            Bids = new List<decimal>(); // Initialize the list of bids
        }

        // Method to receive a bid (ModtagBud) asynchronously
        public async Task<bool> ModtagBudAsync(User buyer, decimal bid)
        {
            return await Task.Run(() =>
            {
                lock (_bidLock) // Ensure thread-safe access to bids
                {
                    if (bid > CurrentPrice)
                    {
                        Bids.Add(bid); // Add the bid to the list
                        CurrentBuyer = buyer; // Set the current buyer to the highest bidder
                        CurrentPrice = bid; // Update the current price to the highest bid
                        Console.WriteLine($"Bid accepted: {bid} from {buyer.Name}");
                        return true;
                    }

                    Console.WriteLine("Bid rejected: Lower than highest bid.");
                    return false;
                }
            });
        }

        // Method to accept the highest bid (AccepterBud) asynchronously
        public async Task<bool> AccepterBudAsync(User seller)
        {
            return await Task.Run(() =>
            {
                lock (_bidLock) // Ensure thread-safe access to the bid and current buyer
                {
                    if (seller == this.Seller && CurrentBuyer != null)
                    {
                        Console.WriteLine($"Bid accepted: {CurrentPrice} from {CurrentBuyer.Name} for Auction {Id}");
                        // Logic to transfer the vehicle to the buyer and complete the sale
                        return true;
                    }

                    Console.WriteLine("No valid bids or seller does not match.");
                    return false;
                }
            });
        }
    }
}
