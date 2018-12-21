using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace GuessTheSongServer.DB
{
    public class DataBaseHandler
    {
        static private DBConnection DBConnection;

        public DataBaseHandler()
        {
            DBConnection = DBConnection.Instance();
            DBConnection.DatabaseName = "GuessTheSong";
            DBConnection.Password = "123456";

            DBConnection.Start();
        }

        public void EndDBConnection()
        {
            DBConnection.Close();
        }

        public void tryToConnect()
        {
            if (DBConnection.IsConnect())
            {
                //suppose col0 and col1 are defined as VARCHAR in the DB
                string query = "SELECT t.year FROM song t WHERE t.year = 2004";
                var cmd = new MySqlCommand(query, DBConnection.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string someStringFromColumnZero = reader.GetString(0);
                    string someStringFromColumnOne = reader.GetString(1);
                    Console.WriteLine(someStringFromColumnZero + "," + someStringFromColumnOne);
                }
                DBConnection.Close();
            }
        }

    }
}
