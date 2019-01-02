using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MAVLink.NET
{
    class DatabaseManager
    {
        private static string server    = "localhost";
        private static int port         = 3307;

        private static string database  = "uavdb";
        private static string user      = "root";
        private static string password  = "sb123!@#";

        public static MySqlConnection GetConnection()
        {
            string config = String.Format("Server={0:s};Port={1:d};Database={2:s};Uid={3:s};Pwd={4:s}",
                                            server, port, database, user, password);
            return new MySqlConnection(config);
        }
    }
}
