using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheSongServer.DB
{
    public class QuestionsManager
    {
        private string artist;
        private string genre;
        private int year;
        private int decadeStart;
        private int decadeEnd;
        private DataBaseHandler dbHandler;

        public QuestionsManager(string artist, string genre, int year, DataBaseHandler dbHandler)
        {
            this.artist = artist;
            this.genre = genre;
            this.year = year;
            decadeStart = (year / 10) * 10;
            decadeEnd = decadeStart + 10;
            this.dbHandler = dbHandler;
        }

        public List<Song> AskQuestion()
        {
            string query = String.Format(QuestionQueries.easyQuery, genre, artist, decadeStart, decadeEnd);
            return dbHandler.GetSongs(query);
        }
    }
}
