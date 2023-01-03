using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class SmartContract
    {

       
        
        public List<Block> blockchain = new List<Block>();
        public List<User> users = new List<User>(); 
        public List<Miner> miners = new List<Miner>();


        private User defaultUser = null;
        User StartUser = new User("System User");
        Random random = new Random();
        private Block lastBlock = null;
        public void UIHandler()
        {
           
            users.Add(StartUser);
            defaultUser = StartUser;

            Miner startMiner = new Miner();
            miners.Add(startMiner);


            string t;
            do
            {
                Console.WriteLine("1-Pregled Stanja Walleta");
                Console.WriteLine("2-Unos Podataka i majnovanje bloka");
                Console.WriteLine("3-Dodavanje Usera");
                Console.WriteLine("4-Brisanje Usera");
                Console.WriteLine("5-Selekcija Usera");
                Console.WriteLine("X-Izlazak iz programa");

                t = Console.ReadLine();

                switch(t) 
                {
                    case "1":
                        PregledStanja();
                        break;
                    case "2":
                        DataUnos();
                        break;
                    case "3":
                        UserAdd();
                        break;
                    case "4":
                        UserDelete();
                        break;
                    case "5":
                        UserChange();
                        break;

                }
            }
            while ( t.ToUpper() != "X");
        }

        public void PregledStanja()
        {
            Console.WriteLine();
            Console.WriteLine("<------------------PREGLED STANJA WALLETA------------------>");
            foreach (User u in users)
            {
                Console.WriteLine(u.username + ":" + u.BTCbalance + "BTC");
            }
            Console.WriteLine("<--------------------------------------------------------->");
            Console.WriteLine();
        }
        
        public void DataUnos() 
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Unesite podatke:");
            string input = Console.ReadLine();
            Block b = new Block(random.Next(10000).ToString());
            b.data= input;  
         

            int idx = random.Next(miners.Count);

            int ret = miners[idx].Mine(b);

            foreach(Miner m in miners)
            {
                if(m != miners[idx])
                {
                    
                    if(!m.Validate(b, ret))
                    {
                        Console.WriteLine("Blok nije uspesno majnovan");
                        return;
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Blok je uspesno majnovan");
            Console.WriteLine();

            if (blockchain.Count > 0)
            {
                b.Prethodni = lastBlock.ID;
            }
            lastBlock = b;

            double a = random.NextDouble(); 
            defaultUser.BTCbalance += a;
            Console.WriteLine("User " + defaultUser.username + " je uspesno sakupio " + a + " BTC");
            Console.WriteLine();    

            blockchain.Add(b);
        }

        public void UserAdd() 
        {
            Console.WriteLine();
            Console.WriteLine("Unesite username novog usera:");
            string username = Console.ReadLine();
            User u = new User(username);
            users.Add(u);
            Console.WriteLine("Prebaceni ste na usera: " + u.username);
            defaultUser = u;
            Console.WriteLine();
        }

        public void UserDelete() 
        {
            if (defaultUser != StartUser) {
                Console.WriteLine();
                Console.WriteLine("Da li zelite obrisati usera: " + defaultUser.username + " Y/N");
                string input = Console.ReadLine();
                if (input.ToUpper() == "Y")
                {
                    Console.WriteLine("User: " + defaultUser.username + " uspesno obrisan");
                    users.Remove(defaultUser);

                    defaultUser = StartUser;
                    Console.WriteLine("Prebaceni ste na usera: " + defaultUser.username);
                    Console.WriteLine();
                }

            }
        }

        public void UserChange() 
        { 
            Console.WriteLine("Unesite redni broj usera na kog zelite da se prebacite:");
            Console.WriteLine();
            int i = 0;
            foreach(User u in users)
            {
                Console.WriteLine(i +"-" + u.username);
                i++;
            }
            int idx = Int32.Parse(Console.ReadLine());

            if(idx >= 0 && idx < users.Count)
            {
                Console.WriteLine("Prebaceni ste na usera: " + users[idx].username);
                defaultUser = users[idx];
            } else
            {
                Console.WriteLine("User sa tim rednim brojem ne postoji");
            }
        }
    }
}
