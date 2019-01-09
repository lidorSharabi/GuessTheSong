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
        public enum QueryType {SELECT, UPDATE, INSERT};

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


        public MySqlDataReader RunQuery(MySqlCommand command, QueryType type)
        {
            if (DBConnection.IsConnect())
            {
                if (type == QueryType.INSERT)
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        //System.IO.File.WriteAllText(@"C:\Users\User\source\repos\lidorSharabi\GuessTheSong\GuessTheSongServer\Log\error.log.txt", ex.Message);
                    }
                }
                DBConnection.Close();
                return null;
            }
            return null;
        }

        public void SaveUserData(string firstName, string lastName, DateTime? dateOfBirth, int genreID, int artistID)
        {
            int score = 0;
            DateTime? lastModified = DateTime.Now;
            string query = "INSERT INTO users (FirstName, LastName, DateOfBirth, Genre, Artist, Score, LastModified)" +
                " Values(@firstName, @lastName, @dateOfBirth, @genre, @artist, @score, @lastModified)";
            
            MySqlCommand command = new MySqlCommand(query, DBConnection.Connection);
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@lastName", lastName);
            command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
            command.Parameters.AddWithValue("@genre", genreID);
            command.Parameters.AddWithValue("@artist", artistID);
            command.Parameters.AddWithValue("@score", score);
            command.Parameters.AddWithValue("@lastModified", lastModified);

            RunQuery(command, DataBaseHandler.QueryType.INSERT);
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
