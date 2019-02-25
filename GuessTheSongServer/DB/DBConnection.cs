using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;

namespace GuessTheSongServer.DB
{
    public class DBConnection
    {
        private DBConnection()
        {
        }

        public string Password { get; set; }
        public string User { get; set; }
        public string Server { get; set; }
        
        private string databaseName = string.Empty;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(databaseName))
                    return false;
                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}",Server, DatabaseName, User, Password);
                connection = new MySqlConnection(connstring);
                connection.Open();
            }

            return true;
        }

        public bool Start()
        {
            try
            {
                if (String.IsNullOrEmpty(databaseName))
                    return false;
                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, User, Password);
                connection = new MySqlConnection(connstring);
                connection.Open();
            }
            catch (Exception ex)
            {
                File.AppendAllText(@"GuessTheSongServerLog.txt", "Server DB Error at Start Connection" + ex.Message + Environment.NewLine);
                return false;
            }
            return true;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}