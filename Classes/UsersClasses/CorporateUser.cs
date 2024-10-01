using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes;

public class CorporateUser:User
{
    public decimal Credit {  get; set; }
    public string CVRNumber {  get; set; }
    public CorporateUser(string id, string name, string passWord, string mail, decimal balance, decimal credit, string cvrNumber)
            : base(id, name, passWord, mail, balance)
    {
        Credit = credit;
        CVRNumber = cvrNumber;
    }

    public decimal AvailableFunds
    {
        get
        {
            return Credit + Balance;
        }
    }

    public bool SpendFunds(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be positive.");
        }

        if (amount <= Balance)
        {
            // Deduct from balance if there's enough
            Balance -= amount;
            return true;
        }
        else if (amount <= AvailableFunds)
        {
            // Deduct the entire balance and take the rest from credit
            decimal remainingAmount = amount - Balance;
            Balance = 0; // Balance fully used
            Credit -= remainingAmount; // Deduct the remainder from credit
            return true;
        }
        else
        {
            // Not enough funds available
            return false;
        }
    }
    public override string ToString()
    {
        return base.ToString() + $", Credit: {Credit}, CVR Number: {CVRNumber}";
    }
}
