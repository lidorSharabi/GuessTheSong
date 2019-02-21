using GuessTheSongServer.DM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheSongServer.DB
{
    public class QuestionsManager
    {
        private int artistID;
        private int genreID;
        private int year;
        private int decadeStart;
        private int decadeEnd;
        private DataBaseHandler dbHandler;

        public QuestionsManager(int artistID, int genreID, int year, DataBaseHandler dbHandler)
        {
            this.artistID = artistID;
            this.genreID = genreID;
            this.year = year;
            decadeStart = (year / 10) * 10;
            decadeEnd = decadeStart + 10;
            this.dbHandler = dbHandler;
        }

        private List<Record> TurnSongsIntoCorrectRecords(List<Song> songs)
        {
            List<Record> records = new List<Record>();
            foreach (Song song in songs)
            {
                records.Add(new Record() { Song = song, Correctness = true });
            }
            return records;
        }

        private List<Record> TurnSongsIntoWrongRecords(List<Song> songs)
        {
            List<Record> records = new List<Record>();
            foreach (Song song in songs)
            {
                records.Add(new Record() { Song = song, Correctness = false });
            }
            return records;
        }

        public List<Record> AskQuestion(int level)
        {
            string query1, query2;
            List<Song> songs1 = new List<Song>();
            List<Song> songs2 = new List<Song>();

            switch (level)
            {
                case 1:
                    query1 = String.Format(QuestionQueries.artist_genre_years_Query, artistID, genreID, decadeStart, decadeEnd);
                    songs1 = dbHandler.GetSongs(query1);
                    query2 = String.Format(QuestionQueries.artist_diffGenre_Query, artistID, genreID);
                    songs2 = dbHandler.GetSongs(query2);
                    if (songs2.Count == 0)
                    {
                        query2 = String.Format(QuestionQueries.artist_genre_widerYears_Query, artistID, genreID, decadeStart, decadeEnd);
                        songs2 = dbHandler.GetSongs(query2);
                    }
                    break;
                case 2:
                    query1 = String.Format(QuestionQueries.artist_genre_widerYears_Query, artistID, genreID, decadeStart, decadeEnd);
                    songs1 = dbHandler.GetSongs(query1);
                    query2 = String.Format(QuestionQueries.mostPopularArtist_genre_years, genreID, decadeStart, decadeEnd);
                    songs2 = dbHandler.GetSongs(query2);
                    break;
                case 3:
                    query1 = String.Format(QuestionQueries.mostPopularArtist_genre_years, genreID, decadeStart, decadeEnd);
                    songs1 = dbHandler.GetSongs(query1);
                    query2 = String.Format(QuestionQueries.popularArtist_genre_diffYears, genreID, decadeStart, decadeEnd);
                    songs2 = dbHandler.GetSongs(query2);
                    break;
                default:
                case 4:
                    query1 = String.Format(QuestionQueries.popularArtist_genre_diffYears, genreID, decadeStart, decadeEnd);
                    songs1 = dbHandler.GetSongs(query1);
                    query2 = String.Format(QuestionQueries.diffNonPopularArtist_genre_years, artistID, genreID, decadeStart, decadeEnd);
                    songs2 = dbHandler.GetSongs(query2);
                    break;
            }

            List<Record> records1 = TurnSongsIntoCorrectRecords(songs1);
            List<Record> records2 = TurnSongsIntoWrongRecords(songs2);

            records1.AddRange(records2);
            return records1;
        }
    }
}
