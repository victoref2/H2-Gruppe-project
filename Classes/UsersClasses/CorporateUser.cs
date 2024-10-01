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

    public override string ToString()
    {
        return base.ToString() + $", Credit: {Credit}, CVR Number: {CVRNumber}";
    }
}
