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
        public User CurrentBuyer { get; private set; } // The current highest bidder (user)
        public decimal CurrentPrice { get; private set; } // The current highest bid (price)
        public List<decimal> Bids { get; private set; } // List of bids

        private readonly object _bidLock = new object(); // Lock for bid modification

        public Auction(string id, Vehicle vehicle, User seller, decimal CurrentPrice)
        {
            Id = id;
            Vehicle = vehicle;
            Seller = seller;
            Bids = new List<decimal>(); // Initialize the list of bids
            CurrentPrice = 0m; // Initialize current price to 0
        }

        // Method to receive a bid (ModtagBud) asynchronously
        public async Task<bool> ModtagBudAsync(User buyer, decimal bid)
        {
            return await Task.Run(() =>
            {
                lock (_bidLock) // Ensure thread-safe access to bids
                {
                    if ( bid > CurrentPrice)
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
                    if (seller == this.Seller)
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
