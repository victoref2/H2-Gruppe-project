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
    public string MailNumber { get; set; }

    public User(string id,string name,string passWord,string mailNumber) 
    { 
        Id = id;
        Name = name;
        PassWord = passWord;
        MailNumber = mailNumber;
    }
    public override string ToString() 
    {
        return $"User [ID: {Id}, Name: {Name}, Mail: {MailNumber}, Password: ****]";
    }
}
