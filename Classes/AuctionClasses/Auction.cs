using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes;

public class Auction
{
    public string Id {  get; private set; }
    Vehicle vehicle {get; set; }
    User user {get; set; }
    public int Price {  get; set; }
    public Auction(string id, Vehicle vehicle, User user,int price)
    {
        Id = id;
        Price = price;
    }
}
