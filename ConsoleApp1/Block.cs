using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Block
    {
        private string id;
        private string prethodni;
        private int num;
        public string data { get; set; }


        public Block()
        {
            id = String.Empty;
            prethodni = String.Empty;
         
            data = String.Empty;

        }
        public Block(string id)
        {
  
            ID = id;
            prethodni = String.Empty;
            data = String.Empty;
        }
        public string ID
        {
            get { return id; }
            set { id = value;  }
        }

        public string Prethodni
        {
            get { return prethodni;  }
            set { prethodni = value;  }
        }

        public override string ToString()
        {
            return "ID: " + ID + "\n" + "Prethodni: " + Prethodni + data  + "\n";
        }

        public override int GetHashCode()
        {
            var hashCode = 54321;
            hashCode = hashCode * -232323 + ID.GetHashCode();
            hashCode = hashCode * -232323 + data.GetHashCode();
            hashCode = hashCode * -232323 + Prethodni.GetHashCode();
            hashCode = hashCode * -232323 + Number.GetHashCode();
            hashCode = hashCode * -232323 + data.GetHashCode();
            return hashCode;
        }
        public int Number
        {
            get { return num; }
            set { num = value; }
        }
    }
}
