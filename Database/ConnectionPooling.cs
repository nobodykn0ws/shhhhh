using System;
using System.Data;
using System.Data.OracleClient;
namespace ConsoleApp1.Database
{
    public class ConnectionPooling : IDisposable
    {
        private static IDbConnection instance = null;

        public static IDbConnection GetConnection()
        {
            if (instance == null || instance.State == System.Data.ConnectionState.Closed)
            {
                OracleConnectionStringBuilder ocsb = new OracleConnectionStringBuilder();
                ocsb.DataSource = Database.ConnectionParams.LOCAL_DATA_SOURCE;
                ocsb.UserID = Database.ConnectionParams.USER_ID;
                ocsb.Password = Database.ConnectionParams.PASSWORD;

                ocsb.Pooling = true;
                ocsb.MinPoolSize = 1;
                ocsb.MaxPoolSize = 10;
             
                instance = new OracleConnection(ocsb.ConnectionString);

            }
            return instance;
        }

        public void Dispose()
        {
            if (instance != null)
            {
                instance.Close();
                instance.Dispose();
            }

        }
    }
}
