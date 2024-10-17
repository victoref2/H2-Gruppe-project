using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PassWord { get; set; }
    public string Mail { get; set; }
    public decimal Balance { get; set; }

    public User(int id,string name,string passWord,string mail, decimal balance) 
    { 
        Id = id;
        Name = name;
        PassWord = passWord;
        Mail = mail;
        Balance = balance;
    }

    public override string ToString() 
    {
        return $"User [ID: {Id}, Name: {Name}, Mail: {Mail}, Password: ****]";
    }
}
