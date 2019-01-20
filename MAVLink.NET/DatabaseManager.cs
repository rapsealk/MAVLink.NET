//#define MYSQL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if MYSQL
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

        public static void UpdatePosition(byte systemId, double latitude, double longitude, double altitude, byte nSatellite, ulong gpsTime=0)
        {
            MySqlConnection conn = DatabaseManager.GetConnection();
            try
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand()
                {
                    Connection  = conn,
                    CommandText = "UPDATE realtime SET Lattitude=@lat, Longitude=@lon, SatelliteNumber=@satellite WHERE UAV_ID=@id"
                    // GPSTime=@timestamp
                };
                command.Parameters.AddWithValue("@lat", latitude);
                command.Parameters.AddWithValue("@lon", longitude);
                command.Parameters.AddWithValue("@satellite", nSatellite);
                //command.Parameters.AddWithValue("@timestamp", Gtimestamp);
                command.Parameters.AddWithValue("@id", systemId);
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (MySqlException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        public static void UpdateBattery(byte systemId, int batteryPercentage)
        {
            MySqlConnection conn = GetConnection();
            try
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand()
                {
                    Connection  = conn,
                    CommandText = "UPDATE realtime SET battery=@battery WHERE UAV_ID=@id"
                };
                command.Parameters.AddWithValue("@battery", batteryPercentage);
                command.Parameters.AddWithValue("@id", systemId);
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (MySqlException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        public static void UpdateHeadingDirection(byte systemId, short headingDirection)
        {
            MySqlConnection conn = new MySqlConnection();
            try
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand()
                {
                    Connection  = conn,
                    CommandText = "UPDATE realtime SET HeadingDirection=@heading WHERE UAV_ID=@id"
                };
                command.Parameters.AddWithValue("@heading", headingDirection);
                command.Parameters.AddWithValue("@id", systemId);
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (MySqlException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        public static void UpdateFlightMode(byte systemId, string flightMode)
        {
            MySqlConnection conn = new MySqlConnection();
            try
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand()
                {
                    Connection  = conn,
                    CommandText = "UPDATE realtime SET Flight_Mode=@mode WHERE UAV_ID=@id"
                };
                command.Parameters.AddWithValue("@mode", flightMode);
                command.Parameters.AddWithValue("@id", systemId);
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (MySqlException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        public static void UpdateNextCommand(byte systemId, ushort nextCommand)
        {
            MySqlConnection conn = new MySqlConnection();
            try
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand()
                {
                    Connection  = conn,
                    CommandText = "UPDATE realtime SET command_next=@command WHERE UAV_ID=@id"
                };
                command.Parameters.AddWithValue("@command", nextCommand);
                command.Parameters.AddWithValue("@id", systemId);
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (MySqlException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}
#endif