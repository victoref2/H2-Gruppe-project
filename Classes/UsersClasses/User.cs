using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2_Gruppe_project.Classes;

public class User
{
    public string Id { get; private set; }
    public string Name { get; set; }
    public string PassWord { get; set; }
    public string Mail { get; set; }

    public User(string id,string name,string passWord,string mail) 
    { 
        Id = id;
        Name = name;
        PassWord = passWord;
        Mail = mail;
    }
    public override string ToString() 
    {
        return $"User [ID: {Id}, Name: {Name}, Mail: {MailNumber}, Password: ****]";
    }
}
