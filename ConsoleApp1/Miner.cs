using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Miner
    {
        
        public int Mine(Block b)
        {
            int x = 1;
            while (true)
            {
                b.Number = x;
                string check = String.Format("{0:x8}",b.GetHashCode());
                bool t = true;
               
                for(int i = 0; i < 3; i++)
                {
                    if(check[i] != '0')
                    {
                        t = false;
                        break;
                    }
                }
                if(t == true)
                {
                    return x;
                }
                x++;
            }
        }
        public bool Validate(Block b, int res)
        {
            int test = this.Mine(b);
            return res == test;
        }
    }
}
