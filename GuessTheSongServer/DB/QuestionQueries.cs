using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheSongServer.DB
{
    public static class QuestionQueries
    {
        public static string easyQuery = @"SELECT lyrics.song, lyrics.lyrics FROM guessthesong.artists 
INNER JOIN guessthesong.lyrics
ON artists.id = lyrics.artist
INNER JOIN guessthesong.genres
ON lyrics.genre = genres.id 
WHERE genres.genre = '{0}'
AND artists.filteredName='{1}'
AND (lyrics.year >= '{2}' AND lyrics.year < '{3}')
LIMIT 20";

        public static string easyQuery2 = @"SELECT lyrics.song, lyrics.lyrics FROM guessthesong.artists 
INNER JOIN guessthesong.lyrics
ON artists.id = lyrics.artist
INNER JOIN guessthesong.genres
ON lyrics.genre = genres.id 
WHERE genres.genre = '{0}'
AND artists.filteredName='{1}'
AND (lyrics.year >= '{2}' AND lyrics.year < '{3}')
LIMIT 20";



    }
}
