using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes;

public class CorporateUser:User
{
    public string Credit {  get; set; }
    public string CVRNumber {  get; set; }
    public CorporateUser(string id, string name, string passWord, string mailNumber, string credit, string cvrNumber)
            : base(id, name, passWord, mailNumber)
    {
        Credit = credit;
        CVRNumber = cvrNumber;
    }

    public override string ToString()
    {
        return base.ToString() + $", Credit: {Credit}, CVR Number: {CVRNumber}";
    }
}
