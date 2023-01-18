using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Database;

namespace ConsoleApp1
{
   
    public class SmartContract
    {
        public Miner startMiner = null;

        private User defaultUser = null;
        private static Block lastBlock = null;
        public void Commitaj()
        {
            using (IDbConnection connection = ConnectionPooling.GetConnection())
            {
                connection.Open();
                IDbTransaction trans = connection.BeginTransaction();

                trans.Commit();
            }
        }
        public void PosaljiBitcoin(string idm)
        {
            string query = "update miner set btc = btc + :btc_t where idm = :idm_t";
            double a = random.NextDouble();

            using (IDbConnection connection = ConnectionPooling.GetConnection())
            {
                connection.Open();
                //IDbTransaction transaction = connection.BeginTransaction(); // transaction start
           
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    ParameterUtil.AddParameter(command, "btc_t", DbType.Double);
                    ParameterUtil.AddParameter(command, "idm_t", DbType.String);
                    command.Prepare();
               
                    ParameterUtil.SetParameterValue(command, "idm_t", idm);
                    ParameterUtil.SetParameterValue(command, "btc_t", a);
                    
                  
                    command.ExecuteNonQuery();
                    
                }


               

            }
            
        }

        public void UpdateChain(Block b)
        {
            string query = "insert into chain values (:idb_t, :idpb_t, :broj_t, :sdata_t, :valid_t, :minerid_t)";
  
            using (IDbConnection connection = ConnectionPooling.GetConnection())
            {
                connection.Open();
               

                using (IDbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    ParameterUtil.AddParameter(cmd, "idb_t", DbType.String);
                    ParameterUtil.AddParameter(cmd, "idpb_t", DbType.String);
                    ParameterUtil.AddParameter(cmd, "broj_t", DbType.Int32);
                    ParameterUtil.AddParameter(cmd, "sdata_t", DbType.String);
                    ParameterUtil.AddParameter(cmd, "valid_t", DbType.Int32);
                    ParameterUtil.AddParameter(cmd, "minerid_t", DbType.String);
                    cmd.Prepare();
                    ParameterUtil.SetParameterValue(cmd, "idb_t", b.ID);
                    ParameterUtil.SetParameterValue(cmd, "idpb_t", b.Prethodni);
                    ParameterUtil.SetParameterValue(cmd, "broj_t", b.Number);
                    ParameterUtil.SetParameterValue(cmd, "sdata_t", b.data);
                    ParameterUtil.SetParameterValue(cmd, "valid_t", b.valid);
                    ParameterUtil.SetParameterValue(cmd, "minerid_t", b.idm);

                    cmd.ExecuteNonQuery();
                   
                }

           
            }
        }
            public void UpdateBlock(string idb)
        {
            string query = "update chain set valid = 1 where idb = :idb_t";

            using (IDbConnection connection = ConnectionPooling.GetConnection())
            {
                connection.Open();
              

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    ParameterUtil.AddParameter(command, "idb_t", DbType.Double);
               
                    command.Prepare();
                    ParameterUtil.SetParameterValue(command, "idb_t", idb);
                   
                    command.ExecuteNonQuery();
                }


                

            }
        }

        
        public Miner LoadMiner(string idu)
        {
            string query = "select idm, btc from Miner where userid = :id_t";
            
            Miner m = null;
            using (IDbConnection connection = ConnectionPooling.GetConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    ParameterUtil.AddParameter(command, "id_t", DbType.String);
                    command.Prepare();
                    ParameterUtil.SetParameterValue(command, "id_t", idu);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                           
                             m = new Miner(reader.GetString(0), reader.GetDouble(1));
                            
                        }
                    }
                }
            }
            
            return m;
        }

        public User LoadUser(string name)
        {
            string query = "select idu from userbtc where idu = :id_t" ;
            User u = null;
            using (IDbConnection connection = ConnectionPooling.GetConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    ParameterUtil.AddParameter(command, "id_t", DbType.String);
                  
                    command.Prepare();
                    ParameterUtil.SetParameterValue(command, "id_t", name);
                    

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            
                            u = new User(reader.GetString(0));

                        }
                    }
                }
            }
            return u;
        }

        public static List<Block> LoadChain()
        {
            string query = "select idb, idpb, broj, sdata, valid, minerid from chain";
            List<Block> lista = new List<Block>();
            using (IDbConnection connection = ConnectionPooling.GetConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    

                    command.Prepare();
                   


                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Block b = new Block();
                            b = new Block(reader.GetString(0), reader.GetString(1),
                                reader.GetInt32(2), reader.GetString(3), reader.GetInt32(4), reader.GetString(5));
                            lista.Add(b);

                        }
                    }
                }
            }
            int last = lista.Count() - 1;
            if (lista.Count() > 0)
            {
                lastBlock = lista[last];
            }else
            {
                lastBlock = new Block();
            }
            return lista;
        }


        public List<Block> blockchain = LoadChain();
        public List<User> users = new List<User>(); 
        public List<Miner> miners = new List<Miner>();
     
        
        Random random = new Random();
        
        public void UIHandler()
        {
            
            do
            {
                Console.WriteLine("Unesite username:");
                string username = Console.ReadLine();

               
                defaultUser = LoadUser(username);

            } while (defaultUser == null);

            startMiner = LoadMiner(defaultUser.username);
            miners.Add(startMiner);


            string t;
            do
            {
                Console.WriteLine("1-Pregled Stanja Walleta");
                Console.WriteLine("2-Unos Podataka i majnovanje bloka");
                Console.WriteLine("3-Validiranje Blokova");

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
                        Validacija();
                        break;
          

                }
            }
            while ( t.ToUpper() != "X");
        }

        public void PregledStanja()
        {
            startMiner = LoadMiner(defaultUser.username);
            Console.WriteLine();
            Console.WriteLine("<------------------PREGLED STANJA WALLETA------------------>");

            Console.WriteLine("Username: " + defaultUser.username + " Miner ID: " + startMiner.ID + " Balance: " + startMiner.BTCbalance);
            
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


            int ret = startMiner.Mine(b);
            b.Number = ret;

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Blok je uspesno majnovan");
            Console.WriteLine();
            b.valid = 0;
            b.idm = startMiner.ID;

                b.Prethodni = lastBlock.ID;
            
            lastBlock = b;

           

            blockchain.Add(b);
            UpdateChain(b);
            Commitaj();
        }

        public void Validacija() 
        {
            blockchain = LoadChain();
            foreach(Block b in blockchain)
            {
                if(b.valid == 0 && b.idm != startMiner.ID)
                {
                  
                    b.Number = startMiner.Mine(b);
                   if(startMiner.Validate(b, b.Number))
                    {
                        
                        PosaljiBitcoin(b.idm);
                       
                        Commitaj();
                        b.valid = 1;
                        UpdateBlock(b.ID);
                      
                        Commitaj();
                    }
                }
            }
        }
  
    }
}
