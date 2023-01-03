using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class User
    {
        public string username { get; set; }

        public double BTCbalance { get; set; }

        public User(string username)
        {
            this.username = username;
            BTCbalance = 0;
        }
    }
}
