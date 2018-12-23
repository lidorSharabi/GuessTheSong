using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace GuessTheSongServer.DB
{
    public static class DBActions
    {
        //private DBActions() { }

        public static void SaveUserData(string firstName, string lastName, DateTime? dateOfBirth, string genre, string artist)
        {
            string query = "INSERT INTO users (FirstName, LastName, DateOfBirth, Genre, Artist)" +
                " Values(@firstName, @lastName, @dateOfBirth, @genre, @artist)";
            DataBaseHandler dbHandler = new DataBaseHandler();
            MySqlCommand command = new MySqlCommand(query, dbHandler.DBConnection.Connection);
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@lastName", lastName);
            command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
            command.Parameters.AddWithValue("@genre", genre);
            command.Parameters.AddWithValue("@artist", artist);
            
            dbHandler.RunQuery(command, DataBaseHandler.QueryType.INSERT);
        }
    }
}
