using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuessTheSongServer.DM;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace GuessTheSongServer.DB
{
    public class DataBaseHandler
    {
        static private DBConnection DBConnection;
        public enum QueryType { SELECT, UPDATE, INSERT };

        public DataBaseHandler(string Server, string DatabaseName, string Password, string User)
        {
            DBConnection = DBConnection.Instance();
            DBConnection.DatabaseName = DatabaseName;
            DBConnection.Password = Password;
            DBConnection.Server = Server;
            DBConnection.User = User;


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
                    catch (Exception ex)
                    {
                        //System.IO.File.WriteAllText(@"C:\Users\User\source\repos\lidorSharabi\GuessTheSong\GuessTheSongServer\Log\error.log.txt", ex.Message);
                    }
                }
                //DBConnection.Close();
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

        public List<Genre> GetGenres()
        {
            List<Genre> res = new List<Genre>();
            //DBConnection.Start();
            if (DBConnection.IsConnect())
            {
                //id = 1 is 'Not Available'
                string query = "SELECT * FROM genres WHERE id != 1";
                var cmd = new MySqlCommand(query, DBConnection.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    res.Add(new Genre() { Id = Int32.Parse(reader.GetString(0)), Desc = reader.GetString(1) });
                }
                reader.Close();
            }
            return res;
        }

        public List<Artist> GetArtists()
        {
            List<Artist> res = new List<Artist>();
            //DBConnection.Start();
            if (DBConnection.IsConnect())
            {
                string query = "SELECT t.id, t.artist FROM artists t";
                var cmd = new MySqlCommand(query, DBConnection.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    res.Add(new Artist() { Id = Int32.Parse(reader.GetString(0)), Desc = reader.GetString(1) });
                }
                reader.Close();
            }
            return res;
        }

        public List<Song> GetSongs(string query)
        {
            List<Song> res = new List<Song>();

            if (DBConnection.IsConnect())
            {
                var cmd = new MySqlCommand(query, DBConnection.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    res.Add(new Song() { SongName = reader.GetString(0), Lyric = reader.GetString(1), Correctness = true });
                }
                reader.Close();
            }
            return res;
        }
    }
}
