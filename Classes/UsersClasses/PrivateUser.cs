using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace H2_Gruppe_project.Classes
{
    public class PrivateUser : User
    {
        public string CPRNumber { get; set; }

        public PrivateUser(string id, string name, string passWord, string mail, decimal balance, string cprNumber)
            : base(id, name, passWord, mail, balance)
        {
            CPRNumber = cprNumber;
        }

        public override string ToString()
        {
            return base.ToString() + $", CPR Number: {CPRNumber}";
        }
    }
}
