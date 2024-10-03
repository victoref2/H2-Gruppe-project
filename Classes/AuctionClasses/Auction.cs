﻿using System;
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
        public decimal MinimumPrice { get; set; } // Minimum price for the vehicle
        public List<decimal> Bids { get; private set; } // List of bids

        private Action<decimal> _notifySeller; // Notification method for the seller
        private readonly object _bidLock = new object(); // Lock for bid modification

        public Auction(string id, Vehicle vehicle, User seller, decimal minimumPrice)
        {
            Id = id;
            Vehicle = vehicle;
            Seller = seller;
            MinimumPrice = minimumPrice;
            Bids = new List<decimal>(); // Initialize the list of bids
            CurrentPrice = 0m; // Initialize current price to 0
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

        // Method to receive a bid (ModtagBud) asynchronously
        public async Task<bool> ModtagBudAsync(User buyer, string auctionNumber, decimal bid)
        {
            return await Task.Run(() =>
            {
                lock (_bidLock) // Ensure thread-safe access to bids
                {
                    if (bid >= MinimumPrice && bid > CurrentPrice)
                    {
                        Bids.Add(bid); // Add the bid to the list
                        CurrentBuyer = buyer; // Set the current buyer to the highest bidder
                        CurrentPrice = bid; // Update the current price to the highest bid
                        Console.WriteLine($"Bid accepted: {bid} from {buyer.Name}");

                        // Notify the seller asynchronously
                        Task.Run(() => _notifySeller?.Invoke(bid)); // Run notification in parallel

                        return true;
                    }

                    Console.WriteLine("Bid rejected: Lower than minimum price or current highest bid.");
                    return false;
                }
            });
        }

        // Method to accept the highest bid (AccepterBud) asynchronously
        public async Task<bool> AccepterBudAsync(User seller, string auctionNumber)
        {
            return await Task.Run(() =>
            {
                lock (_bidLock) // Ensure thread-safe access to the bid and current buyer
                {
                    if (CurrentPrice >= MinimumPrice && seller == this.Seller)
                    {
                        Console.WriteLine($"Bid accepted: {CurrentPrice} from {CurrentBuyer.Name} for Auction {auctionNumber}");

                        // Logic to transfer the vehicle to the buyer and complete the sale
                        return true;
                    }

                    Console.WriteLine("No valid bids or seller does not match.");
                    return false;
                }
            });
        }

        // Find auction by ID (FindAuktionMedID) - Could be made async if accessing an external source like a DB
        public static Auction FindAuktionMedID(List<Auction> auctions, string auctionId)
        {
            return auctions.Find(a => a.Id == auctionId); // Find auction by ID
        }
    }
}
