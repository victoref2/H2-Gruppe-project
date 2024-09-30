using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string PassWord { get; set; }
    public string Mail { get; set; }
    public decimal Balance { get; set; }

    public User(string id,string name,string passWord,string mail, decimal balance) 
    { 
        Id = id;
        Name = name;
        PassWord = passWord;
        Mail = mail;
        Balance = 0;
    }

    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);

            return BitConverter.ToString(hash).Replace("-", "").ToLower(); 
        }
    }
    public override string ToString() 
    {
        return $"User [ID: {Id}, Name: {Name}, Mail: {Mail}, Password: ****]";
    }
}
